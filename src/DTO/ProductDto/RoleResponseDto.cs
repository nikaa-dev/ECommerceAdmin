namespace src.DTO.RoleDto
{
    public record RoleResponseDto
    (
        string Id,
        string RoleName,
        string Description,
        string AccessLevel,
        List<string> Permission,
        int Users,
        DateTime Created
    );
}
