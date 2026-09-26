
namespace src.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using src.Models;
using src.Security;
using System.Security.Claims;

public static class DbInitializer
{
    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<DefaultUserConfig> defaultUserConfig)
    {
        // 1. Check if ANY roles exist. If NO roles exist, run the seeder.
        if (!await roleManager.Roles.AnyAsync())
        {
            await SyncRolesAndPermissions(roleManager);
        }

        // 2. Check if ANY users exist. If NO users exist, create the Admin user.
        if (!await userManager.Users.AnyAsync())
        {
            await SeedAdminUserAsync(userManager, defaultUserConfig.Value);
        }
    }

    private static async Task SyncRolesAndPermissions(RoleManager<ApplicationRole> roleManager)
    {
        var allPermissionsInCode = GetAllPermissions().ToList();

        var rolePermissionsMap = new Dictionary<string, List<string>>
        {
            { "Admin", allPermissionsInCode },
            { "Manager", new List<string> {
                "Dashboard::Read", "User::Read",
                "Customer::Read", "Customer::Update", "Customer::Delete", "Customer::Export",
                "Order::Read", "Order::Update", "Order::Export", "Order::Print",
                "Product::Read", "Product::Create", "Product::Update", "Product::Delete", "Product::Export"
            }},
            { "Support", new List<string> {
                "Dashboard::Read", "Customer::Read", "Customer::Update",
                "Order::Read", "Order::Update", "Order::Print", "Product::Read"
            }},
            { "Staff", new List<string> {
                "Dashboard::Read", "Customer::Read",
                "Order::Read", "Order::Update", "Order::Print", "Product::Read"
            }}
        };

        foreach (var roleMap in rolePermissionsMap)
        {
            var roleName = roleMap.Key;
            var targetPermissions = roleMap.Value;

            var role = new ApplicationRole(roleName);
            await roleManager.CreateAsync(role);

            foreach (var permission in targetPermissions)
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }

    private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, DefaultUserConfig defaultAdminUser)
    {
        var adminEmail = defaultAdminUser.Email;

        var user = new ApplicationUser
        {
            UserName = defaultAdminUser.UserName,
            Email = adminEmail,
            EmailConfirmed = true,
            FullName = defaultAdminUser.UserName,
        };

        var result = await userManager.CreateAsync(user, defaultAdminUser.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }

    private static IEnumerable<string> GetAllPermissions()
    {
        var permissions = new List<string>();
        var nestedClasses = typeof(Permissions).GetNestedTypes();
        foreach (var c in nestedClasses)
        {
            var fields = c.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy);
            foreach (var f in fields)
            {
                if (f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                    permissions.Add(f.GetValue(null).ToString());
            }
        }
        return permissions;
    }
}

public class DefaultUserConfig
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public string Password { get; set; } = string.Empty;
}