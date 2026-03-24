using src.DTO.CustomerDto;

namespace src.Services.CustomerServices;

public interface ICustomerService
{
    Task<List<CustomerResponseDto>> GetCustomerIncludedAsync();
}