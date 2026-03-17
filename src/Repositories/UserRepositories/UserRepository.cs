using src.DBConnection;
using src.Models;

namespace src.Repositories.UserRepositories;

public class UserRepository(
    ApplicationDbContext context)
    // UserManager<ApplicationUser> userManager)
    : Repository<ApplicationUser>(context), IUserRepository
{
    // private readonly ApplicationDbContext _context = context;
    // private readonly UserManager<ApplicationUser> _userManager = userManager;
}                                                     