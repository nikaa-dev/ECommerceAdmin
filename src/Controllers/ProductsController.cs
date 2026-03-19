using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;

namespace src.Controllers;
[Authorize(Roles = "Admin,Manager,Staff,Support")]
public class ProductsController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
    
}