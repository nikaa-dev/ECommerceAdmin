using src.DTO.CustomerDto;
using src.Repositories.CustomerRepositories;
using src.Repositories.OrderRepositories;

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
                var orderTotals = orders!
                    .Select(o => o.OrderDetails.Sum(od => od.Quantity * od.Price));

                var response = new CustomerResponseDto()
                {
                    Id = customer.Id,
                    Contact = customer.PhoneNumber,
                    JoinDate = customer.Created,
                    Orders = orders!.Count(),
                    Status = customer.IsActive,
                    TotalSpent = orderTotals.Sum(),
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
}