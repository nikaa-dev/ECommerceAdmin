using src.DBConnection;
using src.Models.Ecommerce;

namespace src.Repositories.CustomerRepositories;

public class CustomerRepository(ApplicationDbContext context):Repository<Customer>(context),ICustomerRepository
{

}