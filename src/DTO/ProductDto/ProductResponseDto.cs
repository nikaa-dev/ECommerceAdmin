using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using src.Enums;

namespace src.DTO.ProductDto;

public record ProductResponseDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, (double)decimal.MaxValue)]
    public decimal Price { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [Required]
    public string Status { get; set; }

    [Required]
    [Range(1,int.MaxValue)] 
    public int Stock { get; set; } = 0;
    public string Category { get; set; }
}
public record ProductDetailResponseDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, (double)decimal.MaxValue)]
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [Required]
    public string Status { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Stock { get; set; } = 0;
    public string Category { get; set; }
}