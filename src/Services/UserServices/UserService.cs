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

            result.Add(new UserResponseDto(
                FullName: user.FullName,
                Email: user.Email!,
                Role: string.Join(", ", roleByUsers),
                Status: user.Status,
                Permission: permissions ,
                JoinDate: DateOnly.FromDateTime(user.CreatedAt),
                LastActive:TimeOnly.FromTimeSpan(TimeSpan.Zero)
            ));
        }
        
        

        return result;
    }

    public async Task AddRolePermissionUserAsync(UserRequestDto userRolePermissionRequestDto)
    {
        try {

            var userName = await userManager.FindByNameAsync(userRolePermissionRequestDto.FullName);
            if (userName == null) throw new Exception("Full Name Not Found");

            var user = await userManager.FindByEmailAsync(userRolePermissionRequestDto.Email);
            if (user == null) throw new Exception("Email Not Found");

            var role = await userManager.AddToRoleAsync(user, userRolePermissionRequestDto.Role);
            if(!role.Succeeded) throw new Exception("Can't add new role to this user");

            user.Status = userRolePermissionRequestDto.Status;
            await userManager.UpdateAsync(user);

        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }

    }

}

// DTO to send to the view
