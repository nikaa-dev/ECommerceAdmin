using System.ComponentModel.DataAnnotations;

namespace src.DTO.AuthDto;

public class LoginDto
{
    [Required]
    public string UsernameOrEmail { get; set; } = string.Empty; // Changed from 'Email'

    [Required]
    public string Password { get; set; } = string.Empty;
}