using src.DTO.ProductDto;
using src.Extensions.Pagenations;
using src.Models.Ecommerce;
using src.Repositories.ProductRepositories;
using System.Text;

namespace src.Services.ProductServices;

public class ProductService(IProductRepository productRepository):IProductService
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
                Category = product.Category!.Name
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
}