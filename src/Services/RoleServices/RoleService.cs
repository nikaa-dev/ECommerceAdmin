using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using src.DBConnection;
using src.DTO.RoleDto;
using src.DTO.UserDto;
using src.Enums;
using src.Models;
using src.Services.RoleServices;
using System.Security;
using System.Security.Claims;
using static src.Enums.Permissions;

namespace src.Services.RoleServices;

public class RoleService(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager ,
    ApplicationDbContext context
    ) : IRoleService
{
    public async Task<List<string?>> GetAllNameAsync()
    {
        var roles = await roleManager.Roles
                                .Select(r => r.Name)
                                .ToListAsync();
        return roles;
    }
                                     
    public async Task<List<RoleResponseDto>> GetAllRoleIncludeAsync()
    {
        var roles = await roleManager.Roles.ToListAsync();
        var result = new List<RoleResponseDto>();

        foreach (var role in roles)
        {
            // Get all users in this role
            var usersInRole = await userManager.GetUsersInRoleAsync(role.Name!);

            // Get permissions
            var permissions = await context.Set<IdentityRoleClaim<string>>()
                .Where(r => r.RoleId == role.Id && r.ClaimType == "Permission")
                .Select(r => r.ClaimValue!)
                .Distinct()
                .ToListAsync();

            // handle access level
            string accessLevel = permissions.Count switch
            {
                >= 20 => "High",
                >= 10 => "Medium",
                _ => "Low"
            };


            result.Add(new RoleResponseDto(
                Id: role.Id,
                RoleName: role.Name,
                Description: role.Description,
                AccessLevel: accessLevel,
                Users: usersInRole.Count,
                Permission: permissions,
                Created: DateTime.UtcNow
            ));
        }

        return result;
    }

    public async Task<Dictionary<string, List<string>>> GetLookupRolePermission()
    {
        var roles = await roleManager.Roles.ToListAsync();
        var rolePermissions = await roleManager.GetClaimsAsync(roles?.FirstOrDefault(r => r.Name == "Admin"));
        var displayPermissions = new Dictionary<string, List<string>>();

        foreach (var rolePermission in rolePermissions)
        {
            var permission = rolePermission.Value.Split("::");

            if (permission.Length > 1)
            {
                var action = char.ToUpper(permission[1][0]) + permission[1][1..].ToLower();
                var displayPermission = $"{permission[0]} {action}";

                if (!displayPermissions.ContainsKey(permission[0]))
                {
                    displayPermissions[permission[0]] = new List<string>();
                }

                displayPermissions[permission[0]].Add(displayPermission);
            }
        }

        return displayPermissions;

    }


    public async Task<(bool status, string messageStatus)> CreateRoleAsync(RoleRequestCreateDto roleRequest)
    {
        // 1. Validate role name
        if (string.IsNullOrWhiteSpace(roleRequest.RoleName))
        {
            return (false, "Role name is required.");
        }

        // 2. Prevent creating Admin manually
        if (roleRequest.RoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            return (false, "Cannot manually create Admin role. It is managed by system seeder.");
        }


        // 3. Check duplicate role
        var roleExist = await roleManager.FindByNameAsync(roleRequest.RoleName);

        if (roleExist != null)
        {
            return (false, "The role name already exists.");
        }


        // 4. Create role
        var role = new ApplicationRole(roleRequest.RoleName)
        {
            Description = roleRequest.Description
        };


        var createResult = await roleManager.CreateAsync(role);

        if (!createResult.Succeeded)
        {
            return (false, "Error occurred while creating role.");
        }


        // 5. Validate permissions
        var validPermissions = GetSystemPermissionsList();


        if (roleRequest.Permission != null && roleRequest.Permission.Any())
        {
            foreach (var permission in roleRequest.Permission)
            {
                var permissionKey = ConvertToPermissionKey(permission);
                // Only add valid permissions
                if (validPermissions.Contains(permissionKey))
                {
                    await roleManager.AddClaimAsync(
                        role,
                        new Claim("Permission", permissionKey)
                    );
                }
            }
        }


        return (true, "Role was created successfully.");
    }

    public async Task<(bool status, string messageStatus)> DeleteRoleAsync(string id)
    {
        // Find role by Id
        var role = await roleManager.FindByIdAsync(id);

        if (role == null)
        {
            return (false, "Role not found.");
        }

        // Prevent deleting system roles
        if (role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            return (false, "Cannot delete Admin role.");
        }

        // Delete role
        var result = await roleManager.DeleteAsync(role);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return (false, $"Failed to delete role: {errors}");
        }

        return (true, "Role deleted successfully.");
    }

    public async Task<(bool status, string messageStatus)> UpdateRole(
     RoleRequestUpdateDto role)
    {
        // Find role by Id
        var existRole = await roleManager.FindByIdAsync(role.Id);

        if (existRole == null)
        {
            return (false, "Role not found.");
        }


        // Prevent updating system Admin role
        if (existRole.Name?.Equals(
                "Admin",
                StringComparison.OrdinalIgnoreCase) == true)
        {
            return (false, "Cannot update Admin role.");
        }


        // -----------------------------------------
        // Update basic role information
        // -----------------------------------------

        existRole.Name = role.RoleName;
        existRole.NormalizedName = role.RoleName.ToUpperInvariant();
        existRole.Description = role.Description;


        var updateResult = await roleManager.UpdateAsync(existRole);

        if (!updateResult.Succeeded)
        {
            var errors = string.Join(
                ", ",
                updateResult.Errors.Select(e => e.Description)
            );

            return (false, errors);
        }


        // -----------------------------------------
        // Get existing permission claims
        // -----------------------------------------

        var existingClaims =
            await roleManager.GetClaimsAsync(existRole);

        var existingPermissions = existingClaims
            .Where(c => c.Type == "Permission")
            .Select(c => c.Value)
            .ToList();


        // -----------------------------------------
        // Get new permissions from frontend
        // -----------------------------------------
        var validPermissions = GetSystemPermissionsList();
        var newPermissions = role.Permission?
            .Select(ConvertToPermissionKey)
            .Where(permission => validPermissions.Contains(permission.ToLower()))
            .Distinct()
            .ToList()
            ?? new List<string>();


        // -----------------------------------------
        // Remove unchecked permissions
        // -----------------------------------------

        foreach (var claim in existingClaims
            .Where(c => c.Type == "Permission"))
        {
            if (!newPermissions.Contains(claim.Value))
            {
                var removeResult =
                    await roleManager.RemoveClaimAsync(
                        existRole,
                        claim
                    );

                if (!removeResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        removeResult.Errors.Select(e => e.Description)
                    );

                    return (false, errors);
                }
            }
        }


        // -----------------------------------------
        // Add newly selected permissions
        // -----------------------------------------

        foreach (var permission in newPermissions)
        {
            if (!existingPermissions.Contains(permission))
            {
                var addResult =
                    await roleManager.AddClaimAsync(
                        existRole,
                        new Claim("Permission", permission)
                    );

                if (!addResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        addResult.Errors.Select(e => e.Description)
                    );

                    return (false, errors);
                }
            }
        }


        return (true, "Role updated successfully.");
    }

    private string ConvertToPermissionKey(string displayPermission)
    {
        var parts = displayPermission.Split(" ");

        if (parts.Length < 2)
            return displayPermission;

        var module = parts[0].ToLower();
        var action = parts[1].ToLower();

        return $"{module}::{action}";
    }

    private List<string> GetSystemPermissionsList()
    {
        var permissions = new List<string>();
        var nestedClasses = typeof(Permissions).GetNestedTypes();

        foreach (var c in nestedClasses)
        {
            var fields = c.GetFields(System.Reflection.BindingFlags.Public |
                                     System.Reflection.BindingFlags.Static |
                                     System.Reflection.BindingFlags.FlattenHierarchy);

            foreach (var f in fields)
            {
                if (f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                {
                    var val = f.GetValue(null)?.ToString();
                    if (!string.IsNullOrEmpty(val)) permissions.Add(val);
                }
            }
        }
        return permissions;

    }
    }