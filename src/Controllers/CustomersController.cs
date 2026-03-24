using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Extensions.Pagenations;
using src.Models;
using src.Services.CustomerServices;

namespace src.Controllers;

public class CustomersController(ILogger<HomeController> logger,ICustomerService customerService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    [Authorize]
    public async Task<IActionResult> Index(int page = 1)
    {
        var customers = await customerService.GetCustomerIncludedAsync();

        ViewBag.activeCustomer = customers.Count(c => c.Status);
        ViewBag.inActiveCustomer = customers.Count(c => c.Status == false);
        
        var queriyable = customers.AsQueryable();
        var paginations = queriyable.ToPagedResultAsync(page, 8);
        return View(paginations);
    }
    
}