using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Enums;
using src.Extensions.Pagenations;
using src.Models;
using src.Services.ProductServices;

namespace src.Controllers;
[Authorize(Roles = "Admin,Manager,Staff,Support")]
public class ProductsController(ILogger<HomeController> logger,IProductService productService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    [Authorize]
    public async Task<IActionResult> Index(string? filterByCategory, ProductStatus? filterByStatus, string? searchItem,int page=1)
    {
        var products = await productService.GetProductListingAsync();
        
        if (filterByCategory != null)
            products = products.Where(p => p.Category == filterByCategory).ToList();
        
        if (filterByStatus != null)
            products = products.Where(p => p.Status == filterByStatus).ToList();
        
        if (searchItem != null)
            products = products.Where(p => p.Name.Contains(searchItem)).ToList();
        
        
        var productQuery = products.AsQueryable();
        var productResults = productQuery.ToPagedResultAsync(page, 8);
        
        return View(productResults);
    }
    
}