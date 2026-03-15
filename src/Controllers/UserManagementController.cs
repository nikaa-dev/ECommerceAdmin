using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.DBConnection;
using src.Services.UserServices;

namespace src.Controllers;

public class UserManagementController : Controller
{
    private readonly ILogger<UserManagementController> _logger;
    private readonly IUserService _userService;

    public UserManagementController(IUserService userService, ILogger<UserManagementController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllIncludeAsync(); 
        return View(users);
    }
}
