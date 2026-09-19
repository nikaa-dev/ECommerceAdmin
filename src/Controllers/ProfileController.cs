using Microsoft.AspNetCore.Mvc;
using src.DTO.UserDto;
using src.Services.UserServices;
using System.Security.Claims;

namespace src.Controllers
{
    public class ProfileController(
        IUserService userService) : Controller
    {
        // GET: /Profile/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var (status, message, user) =
                await userService.GetUserProfile(userId);

            if (!status || user == null)
                return NotFound(message);

            return View(user);
        }


        // GET: /Profile/Update
        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var (status, message, user) =
                await userService.GetUserProfile(userId);

            if (!status || user == null)
                return NotFound(message);

            return View(user);
        }


        // POST: /Profile/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
            ProfileUserRequestDto update)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();


            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return BadRequest(new
                {
                    message = string.Join(" ", errors)
                });
            }


            // =========================
            // Phone validation
            // =========================

            if (!string.IsNullOrEmpty(update.PhoneNumber))
            {
                if (System.Text.RegularExpressions.Regex
                    .IsMatch(update.PhoneNumber, "[a-zA-Z]"))
                {
                    return BadRequest(new
                    {
                        message = "Phone number contains invalid characters."
                    });
                }

                var rawNumber = new string(
                    update.PhoneNumber
                        .Where(char.IsDigit)
                        .ToArray()
                );

                if (rawNumber.Length != 10)
                {
                    return BadRequest(new
                    {
                        message = "Phone number must be exactly 10 digits."
                    });
                }

                update.PhoneNumber = rawNumber;
            }


            // =========================
            // Profile image validation
            // =========================

            if (update.PhotoProfile != null)
            {
                const long maxFileSize = 5 * 1024 * 1024;

                if (update.PhotoProfile.Length > maxFileSize)
                {
                    return BadRequest(new
                    {
                        message = "Profile image must be smaller than 5 MB."
                    });
                }

                var allowedExtensions = new[]
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".webp"
                };

                var extension =
                    Path.GetExtension(
                        update.PhotoProfile.FileName)
                        .ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest(new
                    {
                        message =
                            "Only JPG, JPEG, PNG and WEBP images are allowed."
                    });
                }
            }


            // =========================
            // Update
            // =========================

            var (success, message) =
                await userService.UpdateUserProfile(
                    userId,
                    update);


            if (!success)
            {
                return BadRequest(new
                {
                    message
                });
            }


            return Ok(new
            {
                message = "Profile updated successfully."
            });
        }


        // POST: /Profile/RemovePhoto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePhoto()
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var (success, message) =
                await userService.RemoveProfilePhoto(userId);

            if (!success)
            {
                return BadRequest(new
                {
                    message
                });
            }

            return Ok(new
            {
                message = "Profile photo removed successfully."
            });
        }
    }
}