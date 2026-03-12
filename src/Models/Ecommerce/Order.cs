using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models.Ecommerce;

public class Order
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid OrderStatusId { get; set; }

    public Guid? ShippingAddressId { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }
    
    [ForeignKey("OrderStatusId")]
    public virtual OrderStatus? OrderStatus { get; set; }
    
    [ForeignKey("ShippingAddressId")]
    public virtual Address? ShippingAddress { get; set; }
    
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}