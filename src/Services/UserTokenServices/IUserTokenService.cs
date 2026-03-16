using Microsoft.AspNetCore.Identity;

namespace src.Services.UserTokenServices;

public interface IUserTokenService
{
    Task CreateAsync(IdentityUserToken<string> token);
}