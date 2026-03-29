using Microsoft.EntityFrameworkCore;
using src.DTO.DashboardDto;
using src.Repositories.CustomerRepositories;
using src.Repositories.OrderDetailRepositories;
using src.Repositories.OrderRepositories;
using src.Repositories.ProductRepositories;
using src.Repositories.UserRepositories;
using src.Services.CustomerServices;
using src.Services.OrderServices;
using src.Services.ProductServices;
using src.Services.UserServices;
using System.Collections.Immutable;

namespace src.Services.DashboardServices
{
    public class DashboardService(IOrderRepository orderRepository,
                IUserRepository userRepository,IProductRepository productRepository,
                ICustomerRepository customerRepository,IOrderDetailRepository orderDetailRepository, IOrderService orderService) : IDashboardService
    {
        public async Task<DashboardResponseDto> GetAllAsync()
        {
            var orderTotal = await orderRepository.CountAsync();
            var customerTotal = await customerRepository.CountAsync();
            var productTotal = await productRepository.CountAsync();

            var orderDtos = await orderService.GetAllIncludedAsync();
            var totalRevenue = orderDtos.Sum(o => o.Total);

            var currentYear = DateTime.Now.Year;

            // =======================
            // Orders Per Month
            // =======================
            var ordersGrouped = orderDtos
                .Where(o => o.Date.Year == currentYear)
                .GroupBy(o => o.Date.Month)
                .ToDictionary(g => g.Key, g => g.Count());

            var ordersPerMonth = Enumerable.Range(1, 12)
                .Select(m => new MonthlyDataDto
                {
                    Month = m,
                    Total = ordersGrouped.ContainsKey(m) ? ordersGrouped[m] : 0
                })
                .ToList();

            // =======================
            // Revenue Per Month
            // =======================
            var revenueGrouped = orderDtos
                .Where(o => o.Date.Year == currentYear)
                .GroupBy(o => o.Date.Month)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Total));

            var revenuePerMonth = Enumerable.Range(1, 12)
                .Select(m => new MonthlyDataDto
                {
                    Month = m,
                    Total = revenueGrouped.ContainsKey(m) ? revenueGrouped[m] : 0
                })
                .ToList();

            // =======================
            // Top Products
            // =======================
            var orderDetails = await orderDetailRepository.GetAllAsync();
            var products = await productRepository.GetAllAsync();

            var topProducts = orderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantitySold = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Quantity * x.Price)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(5)
                .Join(products,
                    g => g.ProductId,
                    p => p.Id,
                    (g, p) => new TopProductResponseDto
                    {
                        ProductName = p.Name,
                        RevenuePerProduct = g.TotalRevenue,
                        OrderTotal = g.TotalQuantitySold
                    })
                .ToList();

            // =======================
            // Recent Orders
            // =======================
            var orders = await orderRepository.GetAllAsync();

            var recentOrders = orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .Select(o => new RecentOrderDto
                {
                    Id = o.Id.ToString(),
                    OrderStatus = o.OrderStatus!.Name,
                    CustomerName = o.Customer!.Name,
                    ProductName = o.OrderDetails
                        .Select(od => od.Product!.Name)
                        .FirstOrDefault() ?? "",
                    RevenuePerOrder = o.OrderDetails
                        .Sum(od => od.Quantity * od.Price)
                })
                .ToList();

            // =======================
            // Final Response
            // =======================
            return new DashboardResponseDto
            {
                TotalRevenues = totalRevenue,
                TotalOrders = orderTotal,
                TotalProducts = productTotal,
                TotalCustomers = customerTotal,

                RevenuePerMonth = revenuePerMonth,
                OrdersPerMonth = ordersPerMonth,

                TopProducts = topProducts,
                RecentOrders = recentOrders
            };
        }

    }
}
