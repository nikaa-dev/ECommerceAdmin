using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Services.DashboardServices;

namespace src.Controllers;

[Authorize]
public class DashboardController(ILogger<HomeController> logger,IDashboardService dashboardService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public async Task<IActionResult> Index()
    {
        // var order = await dashboardService.
        return View();
    }
    
}