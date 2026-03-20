using src.Enums;

namespace src.DTO.UserDto;

public record UserResponseDto(
    string FullName,
    string Email,
    string Role,
    UserStatus Status,
    List<string> Permission,
    TimeOnly LastActive,
    DateOnly JoinDate
);
