using src.Models;
namespace src.Services.JwtServices;
public interface IJwtService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
}