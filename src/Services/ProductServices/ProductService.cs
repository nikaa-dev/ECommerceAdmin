using src.DTO.ProductDto;
using src.Extensions.Pagenations;
using src.Models.Ecommerce;
using src.Repositories.ProductCategoryRepositories;
using src.Repositories.ProductRepositories;
using System.Text;

namespace src.Services.ProductServices;

public class ProductService(IProductRepository productRepository,IProductCategoryRepository productCategoryRepository) :IProductService
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

        await productRepository.UpdateAsync(product);
        await productRepository.SaveAsync();

        return true;
    }

    public async Task<bool> CreateProductData(ProductRequestCreateDto request)
    {
        var productExist = await productRepository.FindByNameAsync(request.Name);
        if (productExist != null) return false;

        var category = await productCategoryRepository.GetByNameAsync(request.Category);
        if (category == null) return false;

        var status = request.Status == "Active" ? Enums.ProductStatus.Active
            : request.Status == "OutOfStock" ? Enums.ProductStatus.OutOfStock : Enums.ProductStatus.LowStock;

        Product product = new Product()
        {
            Id = request.Id,
            Name = request.Name,
            Stock = request.Stock,
            Price = request.Price,
            Description = request.Description,
            Category = category,
            CategoryId = category.Id,
            ImageUrl = "Test.png",
            Status = status,
        };

        await productRepository.CreateAsync(product);
        await productRepository.SaveAsync();


        return true;
    }

    public async Task<bool> DeleteProductData(Guid id)
    {
        var product = await productRepository.DeleteAsync(id);
        await productRepository.SaveAsync();

        return product;
    }
}