using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using src.DTO.UserDto;
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
        public async Task<IActionResult> Update()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var (status,message,user) = await userService.GetUserProfile(userId);
            

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProfileUserRequestDto update)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            //    return BadRequest(new { message = string.Join(" ", errors) });
            //}

            var (success, message) = await userService.UpdateUserProfile(userId, update);
            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }




    }
}
