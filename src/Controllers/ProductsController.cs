using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTO.ProductDto;
using src.Enums;
using src.Extensions.Pagenations;
using src.Security;
using src.Services.ProductCategoryServices;
using src.Services.ProductServices;

namespace src.Controllers;

public class ProductsController(ILogger<HomeController> logger,IProductService productService,IProductCategoryService productCategoryService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    [Authorize(Policy = Permissions.Product.Read)]

    public async Task<IActionResult> Index(string? filterByCategory, string? filterByStatus, string? searchItem,int page=1)
    {
        var products = await productService.GetProductListingAsync();
        
        if (filterByCategory != null)
            products = products.Where(p => p.Category == filterByCategory).ToList();
        
        if (filterByStatus != null)
            products = products.Where(p => p.Status == filterByStatus).ToList();

        if (searchItem != null)
            products = products
                        .Where(p => p.Name.ToUpper().Contains(searchItem.ToUpper()))
                        .ToList();

        var category = await productCategoryService.GetAllAsync();
        var status = Enum.GetValues(typeof(ProductStatus)).Cast<ProductStatus>().ToList();
        
        ViewBag.Category = category.Select(c => c.Name);
        ViewBag.ProductStatus = status;
        
        var productQuery = products.AsQueryable();
        var productResults = productQuery.ToPagedResultAsync(page, 8);
        
        return View(productResults);
    }

    [Authorize(Policy = Permissions.Product.Export)]
    public async Task<IActionResult> Export([FromQuery] ProductRequestExportDto request)
    {
        var fileBytes = await productService.ExportProductData(request);
        var fileName = $"product_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        return File(
            fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", // Excel MIME type
            fileName
        );
    }
    [Authorize(Policy = Permissions.Product.Update)]

    public async Task<IActionResult> Update(ProductRequestUpdateDto request)
    {
        if (request == null) return BadRequest("Field is empty!");

        var product = await productService.UpdateProductData(request);
        

        return product == false ? BadRequest(new { success = false, message = "Update failed" })
            : Json(new { success = true, message = "Product Updated successfully" });
    }

    [Authorize(Policy = Permissions.Product.Create)]

    public async Task<IActionResult> Create(ProductRequestCreateDto request)
    {
        var product = await productService.CreateProductData(request);
     

        return product == false ? BadRequest(new { success = false, message = "Create failed" })
            : Json(new { success = true, message = "Product Created successfully" });
    }

    [Authorize(Policy = Permissions.Product.Delete)]

    public async Task<IActionResult> Delete(Guid Id)
    {
        
        var product = await productService.DeleteProductData(Id);

        return product == false ? BadRequest(new { success = false, message = "Delete failed" })
            : Json(new { success = true, message = "Product Deleted successfully" });
    }


}