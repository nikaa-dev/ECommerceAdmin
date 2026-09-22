using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Security;
using src.Services.DashboardServices;
using System.Diagnostics;

namespace src.Controllers;

[Authorize]
public class DashboardController(ILogger<HomeController> logger,IDashboardService dashboardService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    [Authorize(Policy = Permissions.Dashboard.Read)]

    public async Task<IActionResult> Index()
    {
        var order = await dashboardService.GetAllAsync();
        return View(order);
    }
    
}