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
    public async Task<List<Order>?> GetOrderIdAsync(Guid orderId)
    {
        var orders = await context.Orders.Where(o => o.Id == orderId)
                                    .Include(o => o.Customer)
                                    .Include(o => o.OrderDetails)
                                        .ThenInclude(od => od.Product)
                                    .Include(o => o.OrderStatus)
                                    .ToListAsync();
        return orders;
    }

    public async Task<List<Order>> GetAllIncludedAsync()
    {
        var orders = await context.Orders
                                .Include(o => o.Customer)
                                .Include(o => o.OrderDetails)
                                .Include(o => o.OrderStatus)
                                .ToListAsync();
        return orders;
    }
}