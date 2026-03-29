using src.DBConnection;
using src.Models.Ecommerce;

namespace src.Repositories.OrderDetailRepositories
{
    public class OrderDetailRepository(ApplicationDbContext context):Repository<OrderDetail>(context),IOrderDetailRepository
    {

    }
}
