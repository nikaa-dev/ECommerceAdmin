using Microsoft.AspNetCore.Identity;

namespace src.Services.UserTokenServices;

public interface IUserTokenService
{
    Task CreateAsync(IdentityUserToken<string> token);
    Task UpdateAsync(IdentityUserToken<string> token);
    new Task<IdentityUserToken<string>?> FindAsync(string userId, string loginProvider, string name);
}