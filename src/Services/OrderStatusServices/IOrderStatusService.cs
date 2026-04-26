using src.DTO.OrderStatusDto;

namespace src.Services.OrderStatusServices;

public interface IOrderStatusService
{
    Task<List<OrderStatusResponseDto>> GetAllAsync();
}