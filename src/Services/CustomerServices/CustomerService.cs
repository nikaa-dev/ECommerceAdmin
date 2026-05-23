
using Microsoft.AspNetCore.Mvc;
using src.DTO.CustomerDto;
using src.Extensions.Pagenations;
using src.Models.Ecommerce;
using src.Repositories.CustomerRepositories;
using src.Repositories.OrderRepositories;
using System.Collections.Generic;
using System.Text;

namespace src.Services.CustomerServices;

public class CustomerService(ICustomerRepository customerRepository,IOrderRepository orderRepository):ICustomerService
{
    public async Task<List<CustomerResponseDto>> GetCustomerIncludedAsync()
    {
        try
        {
            var customers = await customerRepository.GetAllAsync();
            var responses = new List<CustomerResponseDto>();

            foreach (var customer in customers)
            {
                var orders = await orderRepository.GetByCustomerIdAsync(customer.Id);
                var orderTotals = orders.Sum(o => o.TotalAmount);

                var response = new CustomerResponseDto()
                {
                    Id = customer.Id,
                    Contact = customer.PhoneNumber,
                    JoinDate = customer.Created,
                    Orders = orders!.Count(),
                    Status = customer.IsActive,
                    TotalSpent = orderTotals,
                    Name = customer.Name,
                    Email = customer.Email
                };
                responses.Add(response);
            }
            return responses;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<bool> UpdateCustomerAsync(CustomerRequestUpdateDto customerRequestUpdateDto)
    {
        var update = await customerRepository.GetByIdAsync(customerRequestUpdateDto.Id);
        if (update == null) return false;

        update.Email = customerRequestUpdateDto.Email;
        update.PhoneNumber = customerRequestUpdateDto.Phone;
        update.Name = customerRequestUpdateDto.FullName;
        update.IsActive = customerRequestUpdateDto.Status == "Active" ? true : false;

        await customerRepository.UpdateAsync(update);
        await customerRepository.SaveAsync();

        return true;
    }
    public async Task<bool> DeleteCustomerAsync(string id) 
    {
        var delete = await customerRepository.GetByIdAsync(id);
        if (delete == null) return false;

        await customerRepository.DeleteAsync(Guid.Parse(id));
        await customerRepository.SaveAsync();

        return true;

    }
    //public async Task<bool> CustomerExportAsync(List<CustomerRequestExportDto> items)
    //{

    //    using (var workbook = new XLWorkbook())
    //    {
    //        // ?????? Worksheet ????????????????? (ClosedXML ???????? Property ???????? Column ????????????????)
    //        var worksheet = workbook.Worksheets.Add(sheetName);
    //        worksheet.Cell(1, 1).InsertTable(data);
    //        worksheet.Columns().AdjustToContents(); // ???????????? Column ???????????

    //        using (var stream = new MemoryStream())
    //        {
    //            workbook.SaveAs(stream);
    //            return stream.ToArray();
    //        }
    //    }

    //}

    public async Task<FileContentResult> ExportCustomerData(CutomerRequestExportDto pagination)
    {
        // find path for export
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var downloadPath = Path.Combine(userProfile, "Downloads");

        var fileName = $"customer_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";

        var fullPath = Path.Combine(downloadPath, fileName);



        // get row data
        var customerData = await GetCustomerIncludedAsync();
        var customerDataQueryable = customerData.AsQueryable();
        var customerPagination = customerDataQueryable.ToPagedResultAsync(pagination.PageNumber, pagination.Count);

        // get all properties dynamically
        var properties = typeof(CustomerResponseDto).GetProperties();

        StringBuilder builder = new StringBuilder();

        // create header
        builder.AppendLine(string.Join(",",properties.Select(p => p.Name)));

        // create row
        foreach (var cusPage in customerPagination.Items)
        {
            var row = properties.Select(p =>
            {
                var value = p.GetValue(cusPage);

                return value?.ToString()?.Replace(",", " ");
            });

            builder.AppendLine(string.Join(",", row));
        }

        // export file
        await File.WriteAllTextAsync(fullPath, builder.ToString());

        return null;

    }
}