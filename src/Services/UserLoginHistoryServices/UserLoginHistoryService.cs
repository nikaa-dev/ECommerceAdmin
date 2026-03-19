using src.Models;
using src.Repositories.UserLoginHistoryRepositories;

namespace src.Services.UserLoginHistoryServices;

public class UserLoginHistoryService(IUserLoginHistoryRepository loginHistoryRepository):IUserLoginHistoryService
{
    public async Task CreateAsync(UserLoginHistory loginHistory)
    {
        try
        {
            await loginHistoryRepository.CreateAsync(loginHistory);
            await loginHistoryRepository.SaveAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}