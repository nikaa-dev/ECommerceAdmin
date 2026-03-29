using src.Enums;
using src.Models.Ecommerce;

namespace src.DTO.DashboardDto;

public class DashboardResponseDto
{
    public decimal TotalRevenues { get; set; }
    public decimal TotalOrders { get; set; }
    public decimal TotalProducts { get; set; }
    public decimal TotalCustomers { get; set; }

    public List<MonthlyDataDto> RevenuePerMonth { get; set; }
    public List<MonthlyDataDto> OrdersPerMonth { get; set; }

    public List<RecentOrderDto> RecentOrders { get; set; }
    public List<TopProductResponseDto> TopProducts { get; set; }
}

public class TopProductResponseDto
{
    public string ProductName { get; set; }
    public decimal RevenuePerProduct { get; set; }
    public int OrderTotal { get; set; }
}

public class RecentOrderDto
{
    public string Id { get; set; }
    public string OrderStatus { get; set; }
    public string CustomerName { get; set; }
    public string ProductName { get; set; }
    public decimal RevenuePerOrder { get; set; }
}

public class MonthlyDataDto
{
    public int Month { get; set; }       // 1 ? 12
    public decimal Total { get; set; }
}

