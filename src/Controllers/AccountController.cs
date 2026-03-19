using Microsoft.AspNetCore.Authorization;
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
}