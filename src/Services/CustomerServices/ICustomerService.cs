using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using src.DTO.CustomerDto;

namespace src.Services.CustomerServices;

public interface ICustomerService
{
    Task<List<CustomerResponseDto>> GetCustomerIncludedAsync();
    Task<bool> UpdateCustomerAsync(CustomerRequestUpdateDto customerRequestUpdateDto);
    Task<bool> DeleteCustomerAsync(string id);

    Task<FileContentResult> ExportCustomerData(CutomerRequestExportDto request);
}