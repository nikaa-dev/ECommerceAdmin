using Microsoft.AspNetCore.Identity;

namespace src.Repositories.UserTokenRepositories;

public interface IUserTokenRepository:IRepository<IdentityUserToken<string>>
{
    
}