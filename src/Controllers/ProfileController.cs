using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using src.DTO.UserDto;
using src.Models;
using src.Services.UserServices;
using System.Security.Claims;
using System.Text.RegularExpressions;

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

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = string.Join(" ", errors) });
            }

            // Phone Number Validation & Formatting
            if (!string.IsNullOrEmpty(update.PhoneNumber))
            {
                // 1. Check for invalid alphabetical characters
                if (Regex.IsMatch(update.PhoneNumber, "[a-zA-Z]"))
                {
                    return BadRequest(new { message = "Phone number contains invalid characters." });
                }

                // 2. Strip formatting to get just the raw digits
                var rawNumber = new string(update.PhoneNumber.Where(char.IsDigit).ToArray());

                // 3. Validate the length (assuming a standard 10-digit number)
                if (rawNumber.Length != 10)
                {
                    return BadRequest(new { message = "Phone number must be exactly 10 digits." });
                }

                // 4. Update the DTO with the clean 10-digit string to save to the database
                update.PhoneNumber = rawNumber;
            }

            var (success, message) = await userService.UpdateUserProfile(userId, update);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message });
        }



    }
}
