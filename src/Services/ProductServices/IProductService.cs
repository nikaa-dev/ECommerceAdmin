using src.DTO.ProductDto;

namespace src.Services.ProductServices;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetProductListingAsync();
}