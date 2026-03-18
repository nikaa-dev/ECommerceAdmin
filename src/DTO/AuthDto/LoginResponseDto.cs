namespace src.DTO.AuthDto;

public class LoginResponseDto
{
    public string? Token { get; set; }
    public DateTime Expiration { get; set; }
    // public UserDetailDto User { get; set; } = new();
}