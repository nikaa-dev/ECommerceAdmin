using Microsoft.AspNetCore.Identity;
using src.DBConnection;
using src.Models;

namespace src.Repositories.UserTokenRepositories;

public class UserTokenRepository(ApplicationDbContext context): Repository<IdentityUserToken<string>>(context), IUserTokenRepository
{
    
}