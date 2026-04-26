

using src.DBConnection;
using src.Models.Ecommerce;

namespace src.Repositories.ProductCategoryRepositories;

public class ProductCategoryRepository(ApplicationDbContext context):Repository<Category>(context),IProductCategoryRepository
{
    
}