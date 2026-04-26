using src.DTO.DashboardDto;

namespace src.Services.DashboardServices
{
    public interface IDashboardService
    {
        Task<DashboardResponseDto> GetAllAsync();
    }
}
