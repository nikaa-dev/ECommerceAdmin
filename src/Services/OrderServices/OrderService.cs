using src.DTO.OrderDto;
using src.Models.Ecommerce;
using src.Repositories.OrderRepositories;

namespace src.Services.OrderServices;

public class OrderService(IOrderRepository orderRepository):IOrderService
{
    public async Task<List<OrderResponseDto>> GetAllIncludedAsync()
    {
        try
        {
            var orders = await orderRepository.GetAllIncludedAsync();
            return orders.Select(order => new OrderResponseDto()
                {
                    Id = order.Id.ToString(),
                    Status = order.OrderStatus!,
                    Date = order.OrderDate,
                    Item = order.OrderDetails.Sum(od => od.Quantity),
                    Total = order.OrderDetails.Sum(od => od.Price),
                    CustomerName = order.Customer!.Name,
                    CustomerEmail = order.Customer.Email
                })
                .ToList();
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Order>> GetAllAsync()
    {
        var orders = await orderRepository.GetAllAsync();
        return orders;
    }
    public async Task<int> GetCountAsync()
    {
        var total = await orderRepository.GetAllAsync();
        return total.Count();
    }
}