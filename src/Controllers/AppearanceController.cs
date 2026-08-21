using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{
    public class AppearanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
