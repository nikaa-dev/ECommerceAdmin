using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;

namespace src.Controllers;

[Authorize]
public class DashboardController(ILogger<HomeController> logger,IDashboardRepository ) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public async Task<IActionResult> Index()
    {
        var order = await 
        return View();
    }
    
}