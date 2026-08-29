using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTO.UserDto;
using src.Services.UserServices;
using System.Security.Claims;

namespace src.Controllers;

[Authorize]
public class NotificationsController(IUserService userService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return Challenge();

        var (success, _, settings) = await userService.GetNotificationSettings(userId);
        if (!success || settings is null)
            return NotFound();

        return View(settings);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(NotificationSettingsDto settings)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized(new { message = "Your session has expired. Please sign in again." });

        var (success, message) = await userService.UpdateNotificationSettings(userId, settings);
        if (!success)
            return BadRequest(new { message });

        return Ok(new { message });
    }
}
