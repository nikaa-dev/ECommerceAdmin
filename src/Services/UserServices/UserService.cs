using src.Repositories.UserRepositories;
using Microsoft.AspNetCore.Identity;
using src.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using src.DBConnection;
using src.Services;
using src.DTO.AuthDto;
using src.Common;

namespace src.Services.UserServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;
   

    public UserService(
        IUserRepository userRepository,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationDbContext context,
        SignInManager<ApplicationUser> signInManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _signInManager = signInManager;
    }

    // Return a list of users with their roles and permissions
    public async Task<List<UserResponseDto>> GetAllIncludeAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var result = new List<UserResponseDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var permissions = new List<string>();

            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role != null)
                {
                    var rolePermissions = await _context.Set<IdentityRoleClaim<string>>()
                        .Where(r => r.RoleId == role.Id && r.ClaimType == "Permission")
                        .Select(r => r.ClaimValue)
                        .ToListAsync();

                    permissions.AddRange(rolePermissions);
                }
            }

            permissions = permissions.Distinct().ToList();

            result.Add(new UserResponseDto(
                FullName: user.FullName,
                Email: user.Email,
                Role: string.Join(", ", roles),
                Status: !user.LockoutEnabled,
                Permission: permissions ,
                JoinDate: DateOnly.FromDateTime(user.CreatedAt)
            ));
        }

        return result;
    }

    
}

// DTO to send to the view
public record UserResponseDto(
    string FullName,
    string Email,
    string Role,
    bool Status,
    List<string> Permission,
    //TimeOnly LastActive,
    DateOnly JoinDate
);