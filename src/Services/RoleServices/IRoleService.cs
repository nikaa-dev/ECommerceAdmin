namespace src.Services.RoleServices;

public interface IRoleService
{
    Task<List<string?>> GetAllNameAsync();
}