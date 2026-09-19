namespace src.DTO.RoleDto;

public class RoleRequestCreateDto
{
    public string RoleName { get; set; } 
    public string Description { get; set; }
    public List<string> Permission { get; set; }
}

public class RoleRequestUpdateDto
{
    public string Id { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    public List<string> Permission { get; set; }
}

public class RoleManagementRequestExportDto
{
    public int PageNumber { get; set; }
    public int Count { get; set; }
}
public class RoleAndPermissionResponseDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedAt { get; set; }
    public List<string> Permissions { get; set; }
    public int UserCount { get; set; }
}            
