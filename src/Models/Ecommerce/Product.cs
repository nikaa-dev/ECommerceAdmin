using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models.Ecommerce;
public class Product
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, (double)decimal.MaxValue)]
    public decimal Price { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [Required]
    public bool Status { get; set; }

    [Required]
    [Range(1,int.MaxValue)] 
    public int Stock { get; set; } = 0;

    // Foreign Key to the Category table
    public Guid CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category? Category { get; set; }
}