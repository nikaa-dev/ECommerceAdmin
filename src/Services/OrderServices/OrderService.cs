using ClosedXML.Excel;
using src.DTO.OrderDto;
using src.DTO.ProductDto;
using src.Extensions.Pagenations;
using src.Models.Ecommerce;
using src.Repositories.OrderRepositories;
using System.Text;

namespace src.Services.OrderServices;

public class OrderService(IOrderRepository orderRepository):IOrderService
{
    public async Task<List<OrderResponseDto>> GetAllIncludedAsync()
    {
        try
        {
            var orders = await orderRepository.GetAllIncludedAsync();

            return orders.Select(order => new OrderResponseDto()
            {
                Id = order.Id.ToString(),
                Code = order.Code,
                Status = order.OrderStatus?.Name ?? "Unknown",
                Date = order.OrderDate,
                Item = order.OrderDetails.Sum(od => od.Quantity),
                Total = order.TotalAmount,
                CustomerName = order.Customer?.Name ?? "No Name",
                CustomerEmail = order.Customer?.Email ?? ""
            })
            .ToList();
        }
        catch (Exception ex)
        {
            // optional logging
            throw;
        }
    }

    public async Task<List<Order>> GetAllAsync()
    {
        var orders = await orderRepository.GetAllAsync();
        return orders;
    }
    public async Task<int> GetCountAsync()
    {
        var total = await orderRepository.GetAllAsync();
        return total.Count();
    }

    public async Task<List<ProductDetailResponseDto>> GetProductByOrderIdAsync(string orderId)
    {
        var guid = Guid.Parse(orderId);
        var orders = await orderRepository.GetOrderIdAsync(guid);

        if (orders == null)
            return new List<ProductDetailResponseDto>();

        return orders
            .SelectMany(o => o.OrderDetails)
            .Where(od => od.Product != null)
            .Select(od => new ProductDetailResponseDto
            {
                Id = od.Product!.Id,
                Name = od.Product.Name,
                Price = od.Product.Price,
                Category = "Null",
                Stock = od.Product.Stock,
                ImageUrl = od.Product.ImageUrl,
                Status = "Null",
                Quantity = od.Quantity
            })
            .ToList();
    }


    public async Task<byte[]> ExportOrderData(OrderRequestExportDto order)
    {
        // get data include
        var orderData = await GetAllIncludedAsync();

        // convert to queryable
        var orderQueryable = orderData.AsQueryable();

        // get data pagination (Added the actual 'await' keyword here)
        var orderPaginate = orderQueryable.ToPagedResultAsync(order.PageNumber, order.Count);

        // define properties
        var properties = typeof(OrderResponseDto).GetProperties();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Orders");

            // --- Create Header Row ---
            for (int i = 0; i < properties.Length; i++)
            {
                var cell = worksheet.Cell(1, i + 1);
                cell.Value = properties[i].Name;

                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Fill.BackgroundColor = XLColor.Teal; // Match the product export styling
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            // --- Insert Data Rows ---
            int currentRow = 2;
            foreach (var item in orderPaginate.Items)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    var cell = worksheet.Cell(currentRow, col + 1);
                    var value = properties[col].GetValue(item);

                    cell.Value = value?.ToString() ?? string.Empty;

                    // MAKE DATA BOLD HERE
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                }
                currentRow++;
            }

            // Adjust column widths automatically
            worksheet.Columns().AdjustToContents();

            // return as bytes
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}