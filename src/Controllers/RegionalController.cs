using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{
    public class RegionalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
