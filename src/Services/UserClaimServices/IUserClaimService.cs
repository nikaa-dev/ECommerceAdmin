using Microsoft.AspNetCore.Identity;

namespace src.Services.UserClaimServices;

public interface IUserClaimService
{
    Task CreateAsync(IdentityUserClaim<string> userClaim);
    Task<IdentityUserClaim<string>?> GetUserClaim(string userId);
}