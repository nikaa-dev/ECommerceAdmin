using Microsoft.AspNetCore.Identity;
namespace src.Models;
public class ApplicationRole : IdentityRole
{
    public string Description { get; set; } = string.Empty;
    public ApplicationRole() : base()
    {
        UserRoles = new HashSet<IdentityUserRole<string>>();
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
    }
    
    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
}