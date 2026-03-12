using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models.Ecommerce;
public class Payment
{
    [Key]
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }
    
    public Guid PaymentMethodId { get; set; }
    
    public Guid PaymentStatusId { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [StringLength(255)]
    public string? TransactionId { get; set; }
    
    // Navigation Properties
    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }
    
    [ForeignKey("PaymentMethodId")]
    public virtual PaymentMethod? PaymentMethod { get; set; }
    
    [ForeignKey("PaymentStatusId")]
    public virtual PaymentStatus? PaymentStatus { get; set; }
}
