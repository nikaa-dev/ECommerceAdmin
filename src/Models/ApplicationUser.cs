using Microsoft.AspNetCore.Identity;
using src.Enums;

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
    // Per-user delivery preferences. Security alerts are deliberately enabled by
    // default and are enforced by the settings endpoint.
    //public bool EmailNotificationsEnabled { get; set; } = true;
    //public bool PushNotificationsEnabled { get; set; } = true;
    //public bool SmsNotificationsEnabled { get; set; }
    //public bool MarketingNotificationsEnabled { get; set; }
    //public bool SecurityAlertsEnabled { get; set; } = true;
    //public bool Status { get; set; }
    public UserStatus Status { get; set; } = UserStatus.Active;
    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
}
