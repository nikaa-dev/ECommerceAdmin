using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Enums;
using src.Services.UserServices;

namespace src.Controllers;

[Authorize(Roles = "Admin")]
public class UserManagementController(IUserService userService, ILogger<UserManagementController> logger)
    : Controller
{
    private readonly ILogger<UserManagementController> _logger = logger;

    public async Task<IActionResult> Index()
    {
        var users = await userService.GetAllIncludeAsync(); 
        return View(users);
    }
}
