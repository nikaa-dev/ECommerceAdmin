using src.DTO.ProductDto;
using src.Extensions.Pagenations;
using src.Models.Ecommerce;
using src.Repositories.ProductCategoryRepositories;
using src.Repositories.ProductRepositories;
using System.Text;
using Microsoft.AspNetCore.Hosting; // Required for IWebHostEnvironment
using Microsoft.AspNetCore.Http;    // Required for IFormFile

namespace src.Services.ProductServices;

public class ProductService(
    IProductRepository productRepository,
    IProductCategoryRepository productCategoryRepository,
    IWebHostEnvironment webHostEnvironment) : IProductService // Injected IWebHostEnvironment
{

    public async Task<List<ProductResponseDto>> GetProductListingAsync()
    {
        var products = await productRepository.ProductIncludeCategory();
        var productResponses = new List<ProductResponseDto>();

        foreach (var product in products)
        {
            var Status = product.Status switch
            {
                Enums.ProductStatus.Active => "Active",
                Enums.ProductStatus.LowStock => "LowStock",
                _ => "OutOfStock"
            };
            var productResponse = new ProductResponseDto()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock,
                Status = Status,
                Category = product.Category!.Name,
                Description = product.Description
            };
            productResponses.Add(productResponse);
        }
        return productResponses;
    }

    public async Task<byte[]> ExportProductData(ProductRequestExportDto request)
    {
        // get data include
        var productData = await GetProductListingAsync();

        // convert to queryable 
        var productQueryable = productData.AsQueryable();

        // get data as pagination
        var productPaginate = productQueryable.ToPagedResultAsync(request.PageNumber, request.Count);

        // set properties
        var properties = typeof(ProductResponseDto).GetProperties();

        // combine string
        StringBuilder builder = new StringBuilder();

        // header
        builder.AppendLine(string.Join(",", properties.Select(p => p.Name)));

        // set value into header
        foreach (var item in productPaginate.Items)
        {
            var row = properties.Select(property =>
            {
                var value = property.GetValue(item);
                return value?.ToString()?.Replace(",", " ");
            });

            builder.AppendLine(string.Join(",", row));
        }

        return Encoding.UTF8.GetBytes(builder.ToString());
    }

    // Helper method to save image to wwwroot/img/products
    private async Task<string> SaveImageAsync(IFormFile image)
    {
        if (image == null || image.Length == 0) return "img/google.png"; // Default image

        // Define path: wwwroot/img/products
        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "img", "products");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        // Generate unique filename to prevent overwriting
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        // Return relative path to store in database
        return "img/products/" + uniqueFileName;
    }

    public async Task<bool> CreateProductData(ProductRequestCreateDto request)
    {
        var productExist = await productRepository.FindByNameAsync(request.Name);
        if (productExist != null) return false;

        var category = await productCategoryRepository.GetByNameAsync(request.Category);
        if (category == null) return false;

        var status = request.Status == "Active" ? Enums.ProductStatus.Active
            : request.Status == "OutOfStock" ? Enums.ProductStatus.OutOfStock : Enums.ProductStatus.LowStock;

        // Process Image
        string imagePath = await SaveImageAsync(request.Image!);

        Product product = new Product()
        {
            Id = request.Id,
            Name = request.Name,
            Stock = request.Stock,
            Price = request.Price,
            Description = request.Description,
            Category = category,
            CategoryId = category.Id,
            ImageUrl = imagePath, // Use the generated path
            Status = status,
        };

        await productRepository.CreateAsync(product);
        await productRepository.SaveAsync();

        return true;
    }

    public async Task<bool> UpdateProductData(ProductRequestUpdateDto request)
    {
        var product = await productRepository.FindByIdIncludeCategory(request.Id);
        if (product == null) return false;

        var category = await productCategoryRepository.GetByNameAsync(request.Category);
        if (category == null) return false;

        product.Name = request.Name;
        product.Status = request.Status == "Active" ? Enums.ProductStatus.Active
            : request.Status == "OutOfStock" ? Enums.ProductStatus.OutOfStock : Enums.ProductStatus.LowStock;

        product.Price = request.Price;
        product.Stock = request.Stock;
        product.Category = category;
        product.Description = request.Description;
        product.CategoryId = category!.Id;

        // Only update image if a new one is uploaded
        if (request.Image != null && request.Image.Length > 0)
        {
            // Update the image path in the database
            product.ImageUrl = await SaveImageAsync(request.Image);
        }

        await productRepository.UpdateAsync(product);
        await productRepository.SaveAsync();

        return true;
    }

    public async Task<bool> DeleteProductData(Guid id)
    {
        // Optional: If you want to delete the physical image file from the server when a product is deleted,
        // you would retrieve the product, get the ImageUrl, use File.Delete(path), and then delete the DB record.

        var product = await productRepository.DeleteAsync(id);
        await productRepository.SaveAsync();

        return product;
    }
}