using src.DTO.OrderDto;
using src.DTO.ProductDto;
using src.Models.Ecommerce;
using src.Repositories.OrderRepositories;
using static src.Enums.Permissions;

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

    public async Task<List<ProductDetailResponseDto>> GetProductByOrderIdAsync(string orderId)
    {
        var guid = Guid.Parse(orderId);
        var orders = await orderRepository.GetOrderIdAsync(guid);

        if (orders == null)
            return new List<ProductDetailResponseDto>();

        return orders
            .SelectMany(o => o.OrderDetails)
            .Where(od => od.Product != null)
            .Select(od => new ProductDetailResponseDto
            {
                Id = od.Product!.Id,
                Name = od.Product.Name,
                Price = od.Product.Price,
                Category = "Null",
                Stock = od.Product.Stock,
                ImageUrl = od.Product.ImageUrl,
                Status = "Null",
                Quantity = od.Quantity
            })
            .ToList();
    }
}