using src.DTO.RoleDto;

namespace src.Services.RoleServices;

public interface IRoleService
{
    Task<(bool status, string messageStatus)> CreateRoleAsync(RoleRequestCreateDto roleRequestCreate);
    Task<List<string?>> GetAllNameAsync();
    Task<List<RoleResponseDto>> GetAllRoleIncludeAsync();

    Task<(bool status, string messageStatus)> DeleteRoleAsync(string id);

    Task<Dictionary<string, List<string>>> GetLookupRolePermission();
}