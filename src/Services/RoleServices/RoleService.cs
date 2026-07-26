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


    public async Task<(bool status, string messageStatus)> CreateRoleAsync(RoleRequestCreateDto roleRequest)
    {
        var roleExist = await roleManager.FindByNameAsync(roleRequest.RoleName);
        if (roleExist == null)
            return (false, "The role name already exist.");
 
        var role = new ApplicationRole(roleRequest.RoleName)
        {
            Description = roleRequest.Description
        };
        var result = await roleManager.CreateAsync(role);
        if (!result.Succeeded)
            return (false, "error occured submit to db.");

        var rolePermission = await roleManager.FindByNameAsync(roleRequest.RoleName);
        if (rolePermission == null) return (false, "role not found");

        if (role.Name == "Admin")
        {
            return (false, "Cannot manually add Admin permissions. They are managed by system seeder." );
        }

        var currentClaims = await roleManager.GetClaimsAsync(role);
        var currentPermissions = currentClaims.Where(c => c.Type == "Permission").ToList();

        foreach (var claim in currentPermissions)
        {
            await roleManager.RemoveClaimAsync(role, claim);
        }

        var validPermissions = GetSystemPermissionsList();
        foreach (var permission in roleRequest.Permission)
        {
            if (validPermissions.Contains(permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
        return (true, "Role was created successfully.");
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