using src.DTO.ProductDto;
using src.Models.Ecommerce;
using src.Repositories.ProductRepositories;

namespace src.Services.ProductServices;

public class ProductService(IProductRepository productRepository):IProductService
{
    public async Task<List<ProductResponseDto>> GetProductListingAsync()
    {
        var products = await productRepository.ProductIncludeCategory();
        var productResponses = new List<ProductResponseDto>();
        foreach (var product in products)
        {
            var productResponse = new ProductResponseDto()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock,
                Status = product.Status,
                Category = product.Category!.Name
            };
            productResponses.Add(productResponse);
        }
        return productResponses;
    }
}