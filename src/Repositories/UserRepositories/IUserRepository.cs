using src.DBConnection;

using src.Models;

namespace src.Repositories.UserRepositories;

public interface IUserRepository:IRepository<ApplicationUser>
{
	Task<List<ApplicationUser>> GetAllIncludeAsync();
}