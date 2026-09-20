
using ClosedXML.Excel;
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

    public async Task<byte[]> ExportCustomerData(CutomerRequestExportDto pagination)
    {
        var customerData = await GetCustomerIncludedAsync();
        var customerQueryable = customerData.AsQueryable();

        // Added missing 'await' here
        var customerPagination =  customerQueryable
            .ToPagedResultAsync(pagination.PageNumber, pagination.Count);

        var properties = typeof(CustomerResponseDto).GetProperties();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Customers");

            // --- Create Header Row ---
            for (int i = 0; i < properties.Length; i++)
            {
                var cell = worksheet.Cell(1, i + 1);
                cell.Value = properties[i].Name;

                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = XLColor.Teal;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            // --- Insert Data Rows ---
            int currentRow = 2;
            foreach (var item in customerPagination.Items)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    var value = properties[col].GetValue(item);
                    worksheet.Cell(currentRow, col + 1).Value = value?.ToString() ?? string.Empty;
                }
                currentRow++;
            }

            // Adjust column widths automatically
            worksheet.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}