
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Enums;
using src.Services.RoleServices;
using src.Services.UserServices;

namespace src.Controllers;

public class UserManagementController(IUserService userService, ILogger<UserManagementController> logger, IRoleService roleService)
    : Controller
{
    private readonly ILogger<UserManagementController> _logger = logger;
    private readonly IRoleService _roleService = roleService;

    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Index()
    {
        var users = await userService.GetAllIncludeAsync();
        var roleNames = await _roleService.GetAllNameAsync();
        ViewBag.RoleNames = roleNames;
        ViewBag.ActiveStatuses = users.Select(u => u.Status).Count(u => u == UserStatus.Active);
        ViewBag.InactiveStatuses = users.Select(u => u.Status).Count(u => u == UserStatus.Inactive);
        ViewBag.SuspendedStatuses = users.Select(u => u.Status).Count(u => u == UserStatus.Suspended);
        
        return View(users);
    }
}
