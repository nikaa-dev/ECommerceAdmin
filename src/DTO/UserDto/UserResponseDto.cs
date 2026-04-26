using src.Enums;

namespace src.DTO.UserDto;

public record UserResponseDto(
    string FullName,
    string Email,
    string Role,
    string Status,
    List<string> Permission,
    TimeOnly LastActive,
    DateOnly JoinDate
);
