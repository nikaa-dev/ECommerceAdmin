using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models.Ecommerce;
public class PaymentStatus
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
}