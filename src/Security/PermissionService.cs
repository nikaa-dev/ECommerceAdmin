using Microsoft.EntityFrameworkCore;
using src.DBConnection; // Adjust this to wherever ApplicationDbContext is
using src.Security;
using System.Linq;
using System.Threading.Tasks;

namespace src.Services.UserServices;

public class PermissionService : IPermissionService
{
    private readonly ApplicationDbContext _context;

    public PermissionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasPermissionAsync(string userId, string requiredPermission)
    {
        bool hasPermission = await (
            from userRole in _context.UserRoles
            where userRole.UserId == userId

            // In C#, it is called RoleClaims (mapping to your RolePermissions table)
            join roleClaim in _context.RoleClaims
                on userRole.RoleId equals roleClaim.RoleId

            // IdentityRoleClaim has 'ClaimType' and 'ClaimValue'. 
            // We check both just in case you saved "Role::Read" in either column.
            where roleClaim.ClaimValue == requiredPermission ||
                  roleClaim.ClaimType == requiredPermission

            select roleClaim
        ).AnyAsync();

        return hasPermission;
    }
}