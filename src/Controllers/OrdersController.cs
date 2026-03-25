using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Enums;
using src.Extensions.Pagenations;
using src.Models;
using src.Models.Ecommerce;
using src.Repositories.OrderRepositories;
using src.Services.OrderServices;
using src.Services.OrderStatusServices;

namespace src.Controllers;
public class OrdersController(ILogger<HomeController> logger,IOrderService orderService,IOrderStatusService orderStatusService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    [Authorize]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
    {
        var orders = await orderService.GetAllIncludedAsync();
        
        ViewBag.Total =  orders.Count();
        ViewBag.Status = await orderStatusService.GetAllAsync();

        ViewBag.Pending = orders.Count(o => o.Status.Name == "Pending");
        ViewBag.Processing = orders.Count(o => o.Status.Name == "Processing");
        ViewBag.Completed = orders.Count(o => o.Status.Name == "Completed");
        ViewBag.Delivered = orders.Count(o => o.Status.Name == "Delivered");
        ViewBag.Cancelled = orders.Count(o => o.Status.Name == "Cancelled");
        
        var queryable = orders.AsQueryable();
        var pagination = queryable.ToPagedResultAsync(page, pageSize);
        return View(pagination);
    }
    
    
}