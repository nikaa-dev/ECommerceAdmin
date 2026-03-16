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
        var user = await _authService.LoginAsync(dto);
        if (user is not null)
        {
            return Redirect($"Dashboard/Index/");
        }
        return Unauthorized("ឈ្មោះ ឬលេខសម្ងាត់មិនត្រឹមត្រូវ");
    }
}