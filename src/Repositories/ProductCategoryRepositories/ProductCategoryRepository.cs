

using Microsoft.EntityFrameworkCore;
using src.DBConnection;
using src.Models.Ecommerce;

namespace src.Repositories.ProductCategoryRepositories;

public class ProductCategoryRepository(ApplicationDbContext context):Repository<Category>(context),IProductCategoryRepository
{

    public async Task<Category?> GetByNameAsync(String name)
    {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Name == name);

        return category!;
    }
    
}