using src.DTO.ProductDto;
using src.Models.Ecommerce;

namespace src.Services.ProductCategoryServices;

public interface IProductCategoryService
{
    Task<List<Category>> GetAllAsync();
}