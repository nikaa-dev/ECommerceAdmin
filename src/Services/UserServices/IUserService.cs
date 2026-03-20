using src.DTO.UserDto;
using src.Enums;
using src.Models;
using UserResponseDto = src.DTO.UserDto.UserResponseDto;

namespace src.Services.UserServices
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllIncludeAsync(string? filterByRole, UserStatus? filterByStatus);
    }
}

