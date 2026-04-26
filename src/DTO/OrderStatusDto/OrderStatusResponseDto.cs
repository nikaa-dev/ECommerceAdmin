using System.ComponentModel.DataAnnotations;

namespace src.DTO.OrderStatusDto;

public class OrderStatusResponseDto
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Description { get; set; }
}