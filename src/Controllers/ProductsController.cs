using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTO.ProductDto;
using src.Enums;
using src.Extensions.Pagenations;
using src.Models;
using src.Services.ProductCategoryServices;
using src.Services.ProductServices;

namespace src.Controllers;
[Authorize(Roles = "Admin,Manager,Staff,Support")]
public class ProductsController(ILogger<HomeController> logger,IProductService productService,IProductCategoryService productCategoryService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    [Authorize]
    public async Task<IActionResult> Index(string? filterByCategory, string? filterByStatus, string? searchItem,int page=1)
    {
        var products = await productService.GetProductListingAsync();
        
        if (filterByCategory != null)
            products = products.Where(p => p.Category == filterByCategory).ToList();
        
        if (filterByStatus != null)
            products = products.Where(p => p.Status == filterByStatus).ToList();
        
        if (searchItem != null)
            products = products.Where(p => p.Name.Contains(searchItem)).ToList();

        var category = await productCategoryService.GetAllAsync();
        var status = Enum.GetValues(typeof(ProductStatus)).Cast<ProductStatus>().ToList();
        
        ViewBag.Category = category.Select(c => c.Name);
        ViewBag.ProductStatus = status;
        
        var productQuery = products.AsQueryable();
        var productResults = productQuery.ToPagedResultAsync(page, 8);
        
        return View(productResults);
    }

    public async Task<IActionResult> Export(ProductRequestExportDto request) {
        var bytes = await productService.ExportProductData(request);

        // define filename
        var fileName = $"product_{DateTime.Now:yyyyMMddHHmmss}.csv";

        return File(bytes,"text/csv", fileName);
    }

    public async Task<IActionResult> Update(ProductRequestUpdateDto request)
    {
        if (request == null) return BadRequest("Field is empty!");

        var product = await productService.UpdateProductData(request);
        

        return product == false ? BadRequest(new { success = false, message = "Update failed" })
            : Json(new { success = true, message = "Product Updated successfully" });
    }

    public async Task<IActionResult> Create(ProductRequestCreateDto request)
    {
        var product = await productService.CreateProductData(request);
     

        return product == false ? BadRequest(new { success = false, message = "Create failed" })
            : Json(new { success = true, message = "Product Created successfully" });
    }

    public async Task<IActionResult> Delete(Guid Id)
    {
        
        var product = await productService.DeleteProductData(Id);

        return product == false ? BadRequest(new { success = false, message = "Delete failed" })
            : Json(new { success = true, message = "Product Deleted successfully" });
    }


}