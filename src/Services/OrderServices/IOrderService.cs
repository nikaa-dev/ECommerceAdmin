using src.DTO.OrderDto;
using src.Models.Ecommerce;

namespace src.Services.OrderServices;

public interface IOrderService
{
    Task<List<OrderResponseDto>> GetAllIncludedAsync();
    Task<List<Order>> GetAllAsync();
    Task<int> GetCountAsync();
}