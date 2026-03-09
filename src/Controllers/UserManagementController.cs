using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using src.Models;

namespace src.Controllers;

public class UserManagementController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public UserManagementController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    
}