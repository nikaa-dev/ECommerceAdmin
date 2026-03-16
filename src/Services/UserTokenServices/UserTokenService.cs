using Microsoft.AspNetCore.Identity;
using src.Repositories;
using src.Repositories.UserTokenRepositories;

namespace src.Services.UserTokenServices;

public class UserTokenService:IUserTokenService
{
    private readonly IUserTokenRepository _userTokenRepository;

    public UserTokenService(IUserTokenRepository userTokenRepository)
    {
        _userTokenRepository = userTokenRepository;
    }

    public async Task CreateAsync(IdentityUserToken<string> token)
    {
        var user = _userTokenRepository.CreateAsync(token);
        await _userTokenRepository.SaveAsync();
    }
}