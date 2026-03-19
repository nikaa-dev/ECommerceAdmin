using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using src.Models;

namespace src.Controllers;

public class ProductsController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Index()
    {
        return View();
    }
    
}