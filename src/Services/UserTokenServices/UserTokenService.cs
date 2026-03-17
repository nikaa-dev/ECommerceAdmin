using Microsoft.AspNetCore.Identity;
using src.Repositories.UserTokenRepositories;

namespace src.Services.UserTokenServices;

public class UserTokenService(IUserTokenRepository userTokenRepository) : IUserTokenService
{
    public async Task CreateAsync(IdentityUserToken<string> token)
    {
        await userTokenRepository.CreateAsync(token);
        await userTokenRepository.SaveAsync();
    }

    public async Task<IdentityUserToken<string>?> FindAsync(string userId, string loginProvider, string name)
    {
        var userToken = await userTokenRepository.FindUserTokenAsync(userId, loginProvider, name);
        return userToken;
    }

    public async Task UpdateAsync(IdentityUserToken<string> token)
    { 
        await userTokenRepository.UpdateAsync(token);
        await userTokenRepository.SaveAsync();
    }
}