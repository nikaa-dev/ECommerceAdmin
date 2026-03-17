namespace src.DTO.AuthDto;
public class UserDetailDto
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public IList<string> Roles { get; set; } = new List<string>();
    public IList<string> Permissions { get; set; } = new List<string>();
}