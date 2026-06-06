using src.DTO.OrderDto;
using src.DTO.ProductDto;
using src.Extensions.Pagenations;
using src.Models.Ecommerce;
using src.Repositories.OrderRepositories;
using System.Text;
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


    public async Task<byte[]> ExportOrderData(OrderRequestExportDto order)
    {
        // get data include
        var orderData = await GetAllIncludedAsync();

        // convert to queryable
        var orderQueryable = orderData.AsQueryable();

        // get data pagination
        var orderPaginate = orderQueryable.ToPagedResultAsync(order.PageNumber, order.Count);

        // define properties
        var properties = typeof(OrderResponseDto).GetProperties();

        // combine string
        StringBuilder builder = new StringBuilder();

        // header
        builder.AppendLine(string.Join(",", properties.Select(p => p.Name)));

        // set value into row
        foreach (var item in orderPaginate.Items)
        {
            var row = properties.Select(property =>
            {
                var value = property.GetValue(item);

                return value?.ToString()?.Replace(",", " ");
            });

            builder.AppendLine(string.Join(",", row));
        }

        // return as bytes
        return Encoding.UTF8.GetBytes(builder.ToString());
    }
}