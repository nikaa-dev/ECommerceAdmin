namespace src.Security;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(string userId, string requiredPermission);
}
