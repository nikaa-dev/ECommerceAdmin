using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using src.Models;
using src.Services.RoleServices;

namespace src.Services.RoleServices;

public class RoleService(RoleManager<ApplicationRole> roleManager) : IRoleService
{
    public async Task<List<string?>> GetAllNameAsync()
    {
        var roles = await roleManager.Roles
                                .Select(r => r.Name)
                                .ToListAsync();
        return roles;
    }
}