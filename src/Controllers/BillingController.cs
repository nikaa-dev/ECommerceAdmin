using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{
    public class BillingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
