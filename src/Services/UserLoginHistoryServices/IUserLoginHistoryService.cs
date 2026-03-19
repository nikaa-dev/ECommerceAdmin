using src.Models;

namespace src.Services.UserLoginHistoryServices;

public interface IUserLoginHistoryService
{
    Task CreateAsync(UserLoginHistory loginHistory);
}