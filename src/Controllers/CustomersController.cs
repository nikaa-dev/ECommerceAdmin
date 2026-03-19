using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Models;

namespace src.Controllers;

public class CustomersController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
    
}