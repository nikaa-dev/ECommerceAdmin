using src.DTO.OrderDto;
using src.DTO.ProductDto;
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
                Total = order.TotalAmount,
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

    public async Task<List<ProductResponseDto>> GetProductByOrderIdAsync(string orderId)
    {
        var guid = Guid.Parse(orderId);
        var order = await orderRepository.GetByIdAsync(guid);

        if (order == null || order.OrderDetails == null)
            return new List<ProductResponseDto>();

        return order.OrderDetails
            .Select(od => new ProductResponseDto
            {
                Id = od!.Product!.Id,
                Name = od.Product.Name,
                Price = od.Product.Price,
                Category = "Null",
                Stock = od.Product.Stock,
                ImageUrl = od.Product.ImageUrl,
                Status = "Null"
            })
            .ToList();
    }
}