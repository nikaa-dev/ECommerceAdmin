using Microsoft.AspNetCore.Identity;

namespace src.Models;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        Id = Guid.NewGuid().ToString();
        SecurityStamp = Guid.NewGuid().ToString();
        UserRoles = new HashSet<IdentityUserRole<string>>();
    }
    public string FullName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Status { get; set; }
    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
}