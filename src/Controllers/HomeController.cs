
using Microsoft.AspNetCore.Mvc;
using src.DTO.AuthDto;
using src.Services.AuthServices;

namespace src.Controllers;

public class HomeController(ILogger<HomeController> logger, IAuthService authService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginDto dto)
    {
        var userLogin = await authService.LoginAsync(dto);
        if (userLogin.Data.Token != null)
        {
            Response.Cookies.Append("AuthToken", userLogin.Data.Token, new CookieOptions
            {
                HttpOnly = true, 
                Secure = true, 
                SameSite = SameSiteMode.Strict, 
                Expires = userLogin.Data.Expiration
            });
            return Redirect("Dashboard/Index/");
        }
        TempData["Message"] = userLogin.Message.ToString();
        return View();
    }
}