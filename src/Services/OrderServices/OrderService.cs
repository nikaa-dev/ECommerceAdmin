using src.Repositories.OrderRepositories;

namespace src.Services.OrderServices;

public class OrderService(IOrderRepository orderRepository):IOrderService
{
    
}