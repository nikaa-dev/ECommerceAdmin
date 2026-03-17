using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using src.DBConnection;

namespace src.Repositories.UserTokenRepositories;

public class UserTokenRepository(ApplicationDbContext context): Repository<IdentityUserToken<string>>(context), IUserTokenRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IdentityUserToken<string>?> FindUserTokenAsync(string userId,string loginProvider,string name)
    {
        var userToken = await _context.Set<IdentityUserToken<string>>()
            .FirstOrDefaultAsync(t => t.UserId == userId 
                                      && t.LoginProvider == loginProvider 
                                      && t.Name == name);
        return userToken;

    }
}