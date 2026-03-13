using src.DBConnection;
using src.Repositories;
using src.Models;
using src.Repositories;
using Microsoft.EntityFrameworkCore; 
using System.Linq; 
using System.Collections.Generic;    
using System.Threading.Tasks;        

namespace src.Repositories.UserRepositories;

public class UserRepository(ApplicationDbContext context, 
                            UserManager<ApplicationUser> userManager,
                            RolerManager<IdentityRole> rolerManager) : Repository<ApplicationUser>(applicationDbContext),IUserRepository
{
    public async Task<List<ApplicationUser>> GetAllIncludeAsync()
    {
        var users = await context.Users
                            // .Include(u => u.UserRoles)
                            //     .ThenInclude(ur => ur.Roles)
                            //         .ThenInclude(r => r.RolePermissions)
                            .ToListAsync();
        return users;
    }
}