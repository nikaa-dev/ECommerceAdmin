using Microsoft.EntityFrameworkCore;
using src.DBConnection;
using src.Models.Ecommerce;

namespace src.Repositories.ProductRepositories;

public class ProductRepository(ApplicationDbContext context):Repository<Product>(context),IProductRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<Product>> ProductIncludeCategory()
    {
        var products = await _context.Products.Include(p => p.Category).ToListAsync();
        return products;
    }

    public async Task<Product?> FindByIdIncludeCategory(Guid id)
    {
        var product = await context.Products
                        .Include(p => p.Category)
                        .FirstOrDefaultAsync(p => p.Id == id);

        return product!;
    }

    public async Task<Product?> FindByNameAsync(string name)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Name == name);
        return product!;
    }
}