using src.Models;
using src.DBConnection;

namespace src.Repositories.RoleRepositories;

public class RoleRepository(ApplicationDbContext applicationDbContext) : Repository<ApplicationRole>(applicationDbContext),IRoleRepository
{
    
}