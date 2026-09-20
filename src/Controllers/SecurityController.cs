using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using src.DTO.SecurityDto;
using src.Models;
using System.Linq;
using System.Threading.Tasks;

namespace src.Controllers
{
    [Authorize]
    public class SecurityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SecurityController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Use this if your form is on a separate /Security/ChangePassword page.
        // If your form is on the Index page, you can delete this GET method.
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Handles the AJAX submission
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (string.IsNullOrEmpty(model.CurrentPassword) || string.IsNullOrEmpty(model.NewPassword))
            {
                return BadRequest(new { message = "Both current and new passwords are required." });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(new { message = "Session expired. Please log in again." });
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                // Refreshes the authentication cookie so the user isn't logged out
                await _signInManager.RefreshSignInAsync(user);

                return Ok(new { message = "Your password has been updated successfully!" });
            }

            // Extract all Identity error messages (e.g., "Password must have at least one uppercase")
            var errorMessage = string.Join(" ", result.Errors.Select(e => e.Description));

            return BadRequest(new { message = errorMessage });
        }
    }
}