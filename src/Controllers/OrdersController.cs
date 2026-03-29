using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Enums;
using src.Extensions.Pagenations;
using src.Models;
using src.Models.Ecommerce;
using src.Repositories.OrderRepositories;
using src.Services.OrderServices;
using src.Services.OrderStatusServices;
using System.Diagnostics;
using static src.Enums.Permissions;

namespace src.Controllers;
public class OrdersController(ILogger<HomeController> logger,IOrderService orderService,IOrderStatusService orderStatusService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    [Authorize]
    public async Task<IActionResult> Index(string? filterByDate, string? searchItem,int page = 1, int pageSize = 8)
    {
        var orders = await orderService.GetAllIncludedAsync();
        
        ViewBag.Total = orders.Count;
        ViewBag.Status = await orderStatusService.GetAllAsync();

        ViewBag.Pending = orders.Count(o => o.Status == "Pending");
        ViewBag.Processing = orders.Count(o => o.Status == "Processing");
        ViewBag.Completed = orders.Count(o => o.Status == "Completed");
        ViewBag.Delivered = orders.Count(o => o.Status == "Delivered");
        ViewBag.Cancelled = orders.Count(o => o.Status == "Cancelled");
        ViewBag.Shipped = orders.Count(o => o.Status == "Shipped");

        if (!string.IsNullOrEmpty(filterByDate))
        {
            var today = DateTime.Now;

            switch (filterByDate.ToLower())
            {
                case "7days":
                    orders = orders.Where(o => o.Date >= today.AddDays(-7)).ToList();
                    break;

                case "30days":
                    orders = orders.Where(o => o.Date >= today.AddDays(-30)).ToList();
                    break;
                case "90days":
                    orders = orders.Where(o => o.Date >= today.AddDays(-90)).ToList();
                    break;

                case "all":
                    // no filtering (keep all orders)
                    break;
            }
        }
        if (!string.IsNullOrEmpty(searchItem))
        {
            orders = orders.Where(o => o.Id.Contains(searchItem)).ToList();
        }

        var queryable = orders.AsQueryable();
        var pagination = queryable.ToPagedResultAsync(page, pageSize);
        return View(pagination);
    }
}