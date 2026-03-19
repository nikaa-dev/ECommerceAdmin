namespace src.Models;
public class UserLoginHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserId { get; set; }

    public DateTime LoginTime { get; set; }

    public string IpAddress { get; set; }

    public string UserAgent { get; set; }

    public ApplicationUser User { get; set; }
}