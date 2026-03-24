using Microsoft.EntityFrameworkCore;
using src.DBConnection;
using src.Models.Ecommerce;

namespace src.Repositories.OrderRepositories;

public class OrderRepository(ApplicationDbContext context):Repository<Order>(context),IOrderRepository
{
    public async Task<List<Order>?> GetByCustomerIdAsync(string customerId)
    {
        var orders = await context.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
        return orders;
    }
}