using src.Models.Ecommerce;

namespace src.Repositories.ProductRepositories;

public interface IProductRepository:IRepository<Product>
{
    Task<List<Product>> ProductIncludeCategory();

    Task<Product?> FindByIdIncludeCategory(Guid id);
    Task<Product?> FindByNameAsync(string name);

}