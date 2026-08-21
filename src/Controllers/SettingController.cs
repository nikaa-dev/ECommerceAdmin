using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace src.Controllers;

public class SettingController:Controller
{
    
    public IActionResult Index(string returnUrl)
    {
        
        return View();
    }
}