using src.DBConnection;
using src.Models.Ecommerce;

namespace src.Repositories.OrderRepositories;

public class OrderRepository(ApplicationDbContext context):Repository<Order>(context),IOrderRepository
{
    
}