using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.Models.Ecommerce;
[Table("Customers")]
public class Customer
{
    [Key] 
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string NormalizedEmail { get; set; }

    [Required]
    public string PasswordHash { get; set; }  

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public DateTime Created { get; set; } = DateTime.UtcNow;

    [Required]
    public bool IsActive { get; set; } = true;
}
