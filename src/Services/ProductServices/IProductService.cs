using src.DTO.ProductDto;

namespace src.Services.ProductServices;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetProductListingAsync();

    Task<byte[]> ExportProductData(ProductRequestExportDto request);

    Task<bool> UpdateProductData(ProductRequestUpdateDto request);

    Task<bool> CreateProductData(ProductRequestCreateDto request);

    Task<bool> DeleteProductData(Guid id);



}