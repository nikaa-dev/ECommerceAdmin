using src.DTO.ProductDto;
using src.Models.Ecommerce;
using src.Repositories.ProductCategoryRepositories;

namespace src.Services.ProductCategoryServices;

public class ProductCategoryService(IProductCategoryRepository repository) : IProductCategoryService
{
    public async Task<List<Category>> GetAllAsync()
    {
        var category = await repository.GetAllAsync();
        return category;
    }
}