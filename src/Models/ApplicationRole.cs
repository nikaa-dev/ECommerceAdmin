using Microsoft.AspNetCore.Identity;

namespace src.Models;
public class ApplicationRole : IdentityRole
{
    public string Description { get; set; } = string.Empty;
    public ApplicationRole() : base()
    {
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
    }
}