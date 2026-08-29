using src.DTO.UserDto;
using src.Enums;
using src.Models;
using UserResponseDto = src.DTO.UserDto.UserResponseDto;

namespace src.Services.UserServices
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllIncludeAsync();
        Task AddRolePermissionUserAsync(UserRequestDto userRolePermissionRequestDto);

        Task<bool> UpdateUser(UserRequestUpdateDto userRequest);
        Task<(bool, string)> CreateUserAsync(UserRequestDto userRequest);
        Task<(bool, string)> DeleteUserAsync(string id);

        Task<(bool, string, ProfileUserResponseDto)> GetUserProfile(string userId);

        Task<(bool, string)> UpdateUserProfile(string userId, ProfileUserRequestDto request);

        Task<(bool, string, NotificationSettingsDto?)> GetNotificationSettings(string userId);
        Task<(bool, string)> UpdateNotificationSettings(string userId, NotificationSettingsDto request);



    }
}

