using src.Services.UserServices;
using src.Repositories.UserRepositories;
using Microsoft.AspNetCore.Identity;
using src.Models;


namespace src.Services.UserServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(IUserRepository userRepository,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<ApplicationUser>> GetAllIncludeAsync()
    {
        // Example: load all users with their roles
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            // You can attach roles to user via a custom DTO or property
        }

        return users;
    }
}
