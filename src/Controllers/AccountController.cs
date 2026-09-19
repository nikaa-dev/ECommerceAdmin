using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace src.Controllers;

public class AccountController:Controller
{
    [AllowAnonymous]
    public IActionResult AccessDenied(string returnUrl)
    {
        // If ReturnUrl exists, redirect back
        if (!string.IsNullOrEmpty(returnUrl))
        {
            ViewBag.ReturnUrl = returnUrl;
        }
        else
        {
            ViewBag.ReturnUrl = Url.Content("~/"); // fallback home page
        }

        return View();
    }
    
    public async Task<IActionResult> Logout(string? returnUrl = null)
    {
        // Sign out Identity authentication
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

        // Remove JWT/access token cookie
        Response.Cookies.Delete("AccessToken");

        // Remove other authentication-related cookies if you have them
        Response.Cookies.Delete(".AspNetCore.Identity.Application");

        return RedirectToAction("Index", "Home");
    }
}