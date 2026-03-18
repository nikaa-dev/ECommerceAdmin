using Microsoft.AspNetCore.Identity;

namespace src.Repositories.UserClaimRepositories;

public interface IUserClaimRepository:IRepository<IdentityUserClaim<string>>
{
    Task<IdentityUserClaim<string>?> GetUserClaimByUserIdAsync(string userId);
}