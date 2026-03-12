using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.DBConnection;

namespace src.Controllers;

public class UserManagementController : Controller
{
    private readonly ILogger<UserManagementController> _logger;
    private readonly ApplicationDbContext _context;

    public UserManagementController(ApplicationDbContext context, ILogger<UserManagementController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        _logger.LogInformation("Fetching all users from database");
        var users = _context.Users.ToList();
        return View(users);
    }
}
