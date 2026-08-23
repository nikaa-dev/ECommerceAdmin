using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Services.UserServices;
using System.Security.Claims;

namespace src.Controllers
{
    public class ProfileController(UserManager<ApplicationUser> userManager,IUserService userService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var (status,message,user) = await userService.GetUserProfile(userId);
            

            return View(user);
        }




    }
}
