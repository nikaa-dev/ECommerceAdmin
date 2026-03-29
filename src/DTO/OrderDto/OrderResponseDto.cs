using src.Models.Ecommerce;

namespace src.DTO.OrderDto;

public class OrderResponseDto
{
    
    public string Id { get; set; }	
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }	
    public DateTime Date { get; set; }
    public int Item { get; set; }
    public decimal Total { get; set; }
    public string? Status { get; set; }
}