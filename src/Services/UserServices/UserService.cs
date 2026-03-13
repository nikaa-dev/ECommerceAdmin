using src.Services.UserServices;
using src.Repositories.UserRepositories;

namespace src.Services.UserServices;

public class UserService(IUserRepository userRepository):IUserService
{
    
}