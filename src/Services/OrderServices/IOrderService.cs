using src.DTO.OrderDto;

namespace src.Services.OrderServices;

public interface IOrderService
{
    Task<List<OrderResponseDto>> GetAllIncludedAsync();
}