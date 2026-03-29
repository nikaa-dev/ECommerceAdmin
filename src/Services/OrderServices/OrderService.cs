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
                Status = order.OrderStatus?.Name ?? "Unknown",
                Date = order.OrderDate,
                Item = order.OrderDetails.Sum(od => od.Quantity),
                Total = order.OrderDetails.Sum(od => od.Price * od.Quantity),
                CustomerName = order.Customer?.Name ?? "No Name",
                CustomerEmail = order.Customer?.Email ?? ""
            })
            .ToList();
        }
        catch (Exception ex)
        {
            // optional logging
            throw;
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