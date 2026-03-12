using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace src.Models.Ecommerce;
public class OrderDetail
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid OrderId { get; set; }
    
    public Guid ProductId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; } // Price at the time of purchase

    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }
}