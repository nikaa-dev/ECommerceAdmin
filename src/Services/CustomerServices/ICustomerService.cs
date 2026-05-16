using src.DTO.CustomerDto;

namespace src.Services.CustomerServices;

public interface ICustomerService
{
    Task<List<CustomerResponseDto>> GetCustomerIncludedAsync();
    Task<bool> UpdateCustomerAsync(CustomerRequestUpdateDto customerRequestUpdateDto);
    Task<bool> DeleteCustomerAsync(string id);

    Task ExportCustomerData(CutomerRequestExportDto request);
}