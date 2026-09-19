using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{
    public class SecurityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
