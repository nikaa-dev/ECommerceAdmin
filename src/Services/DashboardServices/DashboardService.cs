using src.DTO.DashboardDto;
using src.Repositories.CustomerRepositories;
using src.Repositories.OrderRepositories;
using src.Repositories.ProductRepositories;
using src.Repositories.UserRepositories;
using src.Services.CustomerServices;
using src.Services.OrderServices;
using src.Services.ProductServices;
using src.Services.UserServices;

namespace src.Services.DashboardServices
{
    public class DashboardService(IOrderRepository orderRepository,
                IUserRepository userRepository,IProductRepository productRepository,
                ICustomerRepository customerRepository, IOrderService orderService) : IDashboardService
    {                   
        // public async Task<DashboardResponseDto> GetAllAsync()
        // {
        //     var orderTotal = await orderRepository.CountAsync();
        //     var customerTotal = await customerRepository.CountAsync();
        //     var productTotal = await productRepository.CountAsync();
        //     
        //     var orders = await orderService.GetAllIncludedAsync();
        //     var totalRevenue = orders.Sum(o => o.Total);
        //
        //     var currentYear = DateTime.Now.Year;
        //     var ordersPerMonth = orders
        //         .Where(o => o.Date.Year == currentYear)
        //         .GroupBy(o => o.Date.Month)
        //         .Select(g => new {
        //             Month = g.Key,
        //             Total = g.Count()
        //         })
        //         .OrderBy(x => x.Month)
        //         .ToList();
        //     var months = Enumerable.Range(1, 12);
        //     var OrderPerMonth = months.Select(m => new {
        //         Month = m,
        //         Total = ordersPerMonth.FirstOrDefault(x => x.Month == m)?.Total ?? 0
        //     }).ToList();
        //
        //     var TotalRevenuePerMonth = orders
        //         .Where(o => o.Date.Year == currentYear)
        //         .GroupBy(o => o.Date.Month)
        //         .Select(g => new {
        //             Month = g.Key,
        //             Total = g.Sum(x => x.Total)
        //         })
        //         .OrderBy(x => x.Month)
        //         .ToList();
        //     var TotalPerMonth = months.Select(m => new {
        //         Month = m,
        //         Total = ordersPerMonth.FirstOrDefault(x => x.Month == m)?.Total ?? 0
        //     }).ToList();
        //
        //     var topProducts = await productRepository.GetAllAsync();
        //     // topProducts = topProducts.Where(p => p.
        //
        //
        // }

    }
}
