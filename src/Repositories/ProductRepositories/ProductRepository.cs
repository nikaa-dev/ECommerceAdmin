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
}