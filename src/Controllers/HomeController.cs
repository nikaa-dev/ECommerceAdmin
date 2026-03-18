using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.DTO.AuthDto;
using src.Services.AuthServices;

namespace src.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAuthService _authService;
    

    public HomeController(ILogger<HomeController> logger,IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginDto dto)
    {
        var userLogin = await _authService.LoginAsync(dto);
        Response.Cookies.Append("AuthToken", userLogin.Data.Token, new CookieOptions
        {
            HttpOnly = true, // prevents JavaScript access
            Secure = true, // only send over HTTPS
            SameSite = SameSiteMode.Strict, // helps prevent CSRF
            Expires = userLogin.Data.Expiration // set expiry
        });
        TempData["Message"] = userLogin.Message.ToString();
        return Redirect("Dashboard/Index/");
    }
}