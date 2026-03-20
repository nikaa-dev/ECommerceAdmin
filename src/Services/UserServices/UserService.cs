using src.Repositories.UserRepositories;
using Microsoft.AspNetCore.Identity;
using src.Models;
using Microsoft.EntityFrameworkCore;
using src.DBConnection;
using src.DTO.UserDto;
using src.Enums;

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
    public async Task<List<UserResponseDto>> GetAllIncludeAsync(string? filterByRole, UserStatus? filterByStatus)
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
        
        if(filterByRole != null) 
            result = result.Where(r => r.Role == filterByRole).ToList();
        if(filterByStatus != null) 
            result = result.Where(r => r.Status == filterByStatus).ToList();
        
        return result;
    }
}

// DTO to send to the view
