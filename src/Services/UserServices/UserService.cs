using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using src.DBConnection;
using src.DTO.UserDto;
using src.Enums;
using src.Models;
using src.Repositories.UserRepositories;
using static src.Enums.Permissions;

namespace src.Services.UserServices;

public class UserService(
    IUserRepository userRepository,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext context,
    SignInManager<ApplicationUser> signInManager)
    : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;


    // Return a list of users with their roles and permissions
    public async Task<List<UserResponseDto>> GetAllIncludeAsync()
    {
        var users = userManager.Users.ToList();
        var result = new List<UserResponseDto>();

        foreach (var user in users)
        {
            var roleByUsers = await userManager.GetRolesAsync(user);

            var permissions = new List<string>();

            foreach (var roleName in roleByUsers)
            {
                var role = await roleManager.FindByNameAsync(roleName);

                if (role != null)
                {
                    var rolePermissions = await context.Set<IdentityRoleClaim<string>>()
                        .Where(r => r.RoleId == role.Id && r.ClaimType == "Permission")
                        .Select(r => r.ClaimValue)
                        .ToListAsync();

                    permissions.AddRange(rolePermissions!);
                }
            }

            permissions = permissions.Distinct().ToList();

            var userStatus = user.Status == UserStatus.Active ? "Active" : user.Status == UserStatus.InActive ? "InActive" : "Suspended";

            result.Add(new UserResponseDto(
                Id: user.Id,
                FullName: user.FullName,
                Email: user.Email!,
                Role: string.Join(", ", roleByUsers),
                Status: userStatus,
                Permission: permissions,
                JoinDate: DateOnly.FromDateTime(user.CreatedAt),
                LastActive: TimeOnly.FromTimeSpan(TimeSpan.Zero)
            ));
        }

        return result;
    }

    public async Task AddRolePermissionUserAsync(UserRequestDto userRolePermissionRequestDto)
    {
        try
        {
            var message = "Success";
            var userName = await userManager.FindByNameAsync(userRolePermissionRequestDto.FullName);
            if (userName == null) message = "Full Name Not Found";

            var user = await userManager.FindByEmailAsync(userRolePermissionRequestDto.Email);
            if (user == null) message = "Email Not Found";

            var role = await userManager.AddToRoleAsync(user, userRolePermissionRequestDto.Role);
            if (!role.Succeeded) message = "Can't add new role to this user";

            user.Status = userRolePermissionRequestDto.Status switch
            {
                "Active" => UserStatus.Active,
                "InActive" => UserStatus.InActive,
                _ => UserStatus.Suspended
            };
            await userManager.UpdateAsync(user);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    public async Task<bool> UpdateUser(UserRequestUpdateDto userRequest)
    {
        var user = await userManager.FindByIdAsync(userRequest.Id);

        if (user == null)
            return false;
        var guid = Guid.NewGuid();
        user.FullName = userRequest.FullName;
        user.Email = userRequest.Email;
        user.UserName = userRequest.Email;
        user.EmailConfirmed = true;

        user.Status = userRequest.Status switch
        {
            "Active" => UserStatus.Active,
            "InActive" => UserStatus.InActive,
            _ => UserStatus.Suspended
        };
        user.SecurityStamp = guid.ToString();

        var result = await userManager.UpdateAsync(user);

        var currentRoles = await userManager.GetRolesAsync(user);

        await userManager.RemoveFromRolesAsync(user, currentRoles);
        await userManager.AddToRoleAsync(user, userRequest.Role);

        if (!result.Succeeded)
            return false;

        return true;
    }
    public async Task<(bool,string)> CreateUserAsync(UserRequestDto userRequest)
    {
        var user = userManager.FindByEmailAsync(userRequest.Email);
        if (user == null)
            return (false,"the email already register");
        var Status = userRequest.Status switch
        {
            "Active" => UserStatus.Active,
            "InActive" => UserStatus.InActive,
            _ => UserStatus.Suspended
        };
        var emailConfirmed = Guid.NewGuid();
        var newUser = new ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            FullName = userRequest.FullName,
            Email = userRequest.Email,
            UserName = userRequest.Email,
            Status = Status,
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        var createResult = await userManager.CreateAsync(newUser, "P@ssw0rd123!@#");
        if (!createResult.Succeeded)
            return (false, "Create failed");

        var addRoleResult = await userManager.AddToRoleAsync(newUser, userRequest.Role);

        if (!addRoleResult.Succeeded)
            return (false,"Create failed");

        return (true,"User Created");
    }
}


// DTO to send to the view
