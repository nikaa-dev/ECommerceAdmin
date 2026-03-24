using src.Models.Ecommerce;

namespace src.Repositories.OrderRepositories;

public interface IOrderRepository:IRepository<Order>
{
    Task<List<Order>?> GetByCustomerIdAsync(string customerId);
}