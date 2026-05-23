
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
        var customerPagination = customerQueryable
            .ToPagedResultAsync(pagination.PageNumber, pagination.Count);

        var properties = typeof(CustomerResponseDto).GetProperties();

        StringBuilder builder = new StringBuilder();

        // header
        builder.AppendLine(string.Join(",", properties.Select(p => p.Name)));

        // rows
        foreach (var item in customerPagination.Items)
        {
            var row = properties.Select(p =>
            {
                var value = p.GetValue(item);
                return value?.ToString()?.Replace(",", " ");
            });

            builder.AppendLine(string.Join(",", row));
        }

        return Encoding.UTF8.GetBytes(builder.ToString());
    }
}