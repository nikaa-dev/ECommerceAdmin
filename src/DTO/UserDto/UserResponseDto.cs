namespace src.DTO.UserDto;

public record UserResponseDto(
    string FirstName,
    string Email,
    string Role,
    bool Status,
    List<string> Permission,
    TimeOnly LastActive,
    DateOnly JoinDate
);
