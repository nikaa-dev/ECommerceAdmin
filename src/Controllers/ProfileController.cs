using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
