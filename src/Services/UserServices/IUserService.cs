using src.Models;

namespace src.Services.UserServices;

public interface IUserService
{
    Task<List<UserResponseDto>> GetAllIncludeAsync();
}