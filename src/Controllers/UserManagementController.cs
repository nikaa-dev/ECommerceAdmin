
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTO.UserDto;
using src.Enums;
using src.Extensions.Pagenations;
using src.Services.RoleServices;
using src.Services.UserServices;

namespace src.Controllers;

public class UserManagementController(IUserService userService, ILogger<UserManagementController> logger, IRoleService roleService)
    : Controller
{
    private readonly ILogger<UserManagementController> _logger = logger;
    private readonly IRoleService _roleService = roleService;

    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Index(string? filterByRole, UserStatus? filterByStatus, string? searchItem,int page=1)
    {
        var users = await userService.GetAllIncludeAsync();

        var roleNames = await _roleService.GetAllNameAsync();
        var status = Enum.GetValues(typeof(UserStatus)).Cast<UserStatus>().ToList();

        ViewBag.RoleNames = roleNames;
        ViewBag.Status = status;

        ViewBag.Total = users.Count();
        ViewBag.ActiveStatuses = users.Select(u => u.Status).Count(u => u == UserStatus.Active);
        ViewBag.InactiveStatuses = users.Select(u => u.Status).Count(u => u == UserStatus.Inactive);
        ViewBag.SuspendedStatuses = users.Select(u => u.Status).Count(u => u == UserStatus.Suspended);


        if (filterByRole != null)
            users = users.Where(r => r.Role == filterByRole).ToList();
        if (filterByStatus != null)
            users = users.Where(r => r.Status == filterByStatus).ToList();
        if (searchItem != null)
        {
            users = users.Where(r =>
                r.FullName.Contains(searchItem) ||
                r.Email.Contains(searchItem)).ToList();
        }
        var queryable = users.AsQueryable();
        var userPagination = queryable.ToPagedResultAsync(page, 8);
       
        
        return View(userPagination);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRequestDto userRequest)
    {
        try
        {
            await userService.AddRolePermissionUserAsync(userRequest);
        } catch (Exception ex) { 
            Console.WriteLine(ex.ToString());
        }
        return Ok();
    }
}
