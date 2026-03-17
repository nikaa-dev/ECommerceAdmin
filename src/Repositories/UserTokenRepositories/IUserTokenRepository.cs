using Microsoft.AspNetCore.Identity;

namespace src.Repositories.UserTokenRepositories;

public interface IUserTokenRepository:IRepository<IdentityUserToken<string>>
{
    Task<IdentityUserToken<string>?> FindUserTokenAsync(string userId, string loginProvider, string name);
}