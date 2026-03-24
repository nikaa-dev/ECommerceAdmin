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
    public async Task<IActionResult> Index(string? filterByStatus,string? shortBy,string? searchItem,int page = 1)
    {
        var customers = await customerService.GetCustomerIncludedAsync();
        var now = DateTime.Now;
        if (customers != null)
        {
            ViewBag.activeCustomer = customers.Count(c => c.Status);
            ViewBag.inActiveCustomer = customers.Count(c => c.Status == false);
            ViewBag.Avg = customers.Any()
                ? customers.Average(c => c.TotalSpent)
                : 0;


            ViewBag.newMonth = customers
                .Where(c => c.JoinDate.Month == now.Month && c.JoinDate.Year == now.Year)
                .Count();
        }
        if (filterByStatus != null) customers = customers.Where(c => c.Status == true).ToList();

        if (searchItem != null) customers = customers.Where(c => c.Name == searchItem).ToList();

        if (!string.IsNullOrEmpty(shortBy))
        {

            if (shortBy == "Name")
                customers = customers.OrderBy(c => c.Name).ToList();
            if (shortBy == "Order")
                customers = customers.OrderBy(c => c.TotalSpent).ToList();
            if (shortBy == "Spending")
                customers = customers.OrderBy(c => c.TotalSpent).ToList();
        }
        
        var queriyable = customers.AsQueryable();
        var paginations = queriyable.ToPagedResultAsync(page, 8);
        return View(paginations);
    }
    
}