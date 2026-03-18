using Microsoft.AspNetCore.Identity;
using src.Repositories.UserClaimRepositories;

namespace src.Services.UserClaimServices;

public class UserClaimService(IUserClaimRepository userClaimRepository) : IUserClaimService
{
    public async Task CreateAsync(IdentityUserClaim<string> userClaim)
    {
        await userClaimRepository.CreateAsync(userClaim);
        await userClaimRepository.SaveAsync();
    }

    public async Task<IdentityUserClaim<string>?> GetUserClaim(string userId)
    {
        var userClaim = await userClaimRepository.GetUserClaimByUserIdAsync(userId);
        return userClaim;
    }
}
