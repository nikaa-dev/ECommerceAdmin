using src.DBConnection;
using src.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using src.Repositories.UserRepositories;

namespace src.Repositories.UserRepositories;

public class UserRepository : Repository<ApplicationUser>, IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(ApplicationDbContext context,
                          UserManager<ApplicationUser> userManager)
        : base(context)
    {
        _context = context;
        _userManager = userManager;
    }

    
}                                                     