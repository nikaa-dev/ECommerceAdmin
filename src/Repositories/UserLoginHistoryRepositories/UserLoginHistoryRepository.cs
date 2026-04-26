using src.DBConnection;
using src.Models;

namespace src.Repositories.UserLoginHistoryRepositories;

public class UserLoginHistoryRepository(ApplicationDbContext context):Repository<UserLoginHistory>(context),IUserLoginHistoryRepository
{
    
    
}