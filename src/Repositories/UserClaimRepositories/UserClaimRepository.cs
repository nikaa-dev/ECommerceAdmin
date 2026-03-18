using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using src.DBConnection;

namespace src.Repositories.UserClaimRepositories;

public class UserClaimRepository(ApplicationDbContext context):Repository<IdentityUserClaim<string>>(context), IUserClaimRepository
{
    private readonly ApplicationDbContext _context = context;
    public async Task<IdentityUserClaim<string>?> GetUserClaimByUserIdAsync(string userId)
    {
        return await _context.Set<IdentityUserClaim<string>>()
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }
}