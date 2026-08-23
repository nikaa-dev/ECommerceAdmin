using src.Enums;
using src.Models;

namespace src.DTO.UserDto
{
    public record ProfileUserResponseDto
    (
        string Id,
        string FullName,
        string PhotoProfile,
        string PhoneNumber,
        string Email,
        string RoleName,
        string Description,
        UserStatus Status,
        string Address,
        string Company,
        TimeOnly LastActive,
        DateTime JoinDate
    );
     
}
