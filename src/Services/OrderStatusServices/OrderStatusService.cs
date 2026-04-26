using src.DTO.OrderStatusDto;
using src.Repositories.OrderStatusRepositories;

namespace src.Services.OrderStatusServices;

public class OrderStatusService(IOrderStatusRepository orderStatusRepository):IOrderStatusService
{
    public async Task<List<OrderStatusResponseDto>> GetAllAsync()
    {
        try
        {
            var orders = await orderStatusRepository.GetAllAsync();
            return orders.Select(order => new OrderStatusResponseDto()
            {
                Name = order.Name,
                Description = order.Description
            }).ToList();
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}