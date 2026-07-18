using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTO.UserDto;
using src.Enums;
using src.Services.RoleServices;
using src.Services.UserServices;
using src.Extensions.Pagenations;

namespace src.Controllers;

public class RoleManagementController(IUserService userService, ILogger<UserManagementController> logger, IRoleService roleService) : Controller
{

    private readonly ILogger<UserManagementController> _logger = logger;
    private readonly IRoleService _roleService = roleService;

    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Index(string? filterByRole, string? filterByStatus, string? searchItem, int pageNumber = 1)
    {
        var users = await userService.GetAllIncludeAsync();

        var roleNames = await _roleService.GetAllNameAsync();
        var status = Enum.GetValues(typeof(UserStatus)).Cast<UserStatus>().ToList();

        ViewBag.RoleNames = roleNames;
        ViewBag.Status = status;

        ViewBag.Total = users.Count();
        ViewBag.ActiveStatuses = users.Select(u => u.Status).Count(u => u == "Active");
        ViewBag.InactiveStatuses = users.Select(u => u.Status).Count(u => u == "InActive");
        ViewBag.SuspendedStatuses = users.Select(u => u.Status).Count(u => u == "Suspended");

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
        var userPagination = queryable.ToPagedResultAsync(pageNumber, 8);

        return View(userPagination);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRequestDto userRequest)
    {
        try
        {
            var (status, messageStatus) = await userService.CreateUserAsync(userRequest);
            if (!status) return BadRequest(new { success = status, message = messageStatus });
            return Json(new { success = status, message = messageStatus });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }

    }
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var (status, messageStatus) = await userService.DeleteUserAsync(id);
            if (!status) return BadRequest(new { success = status, message = messageStatus });
            return Json(new { success = status, message = messageStatus });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }

    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UserRequestUpdateDto userRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var editUser = await userService.UpdateUser(userRequest);

        if (!editUser)
        {
            return BadRequest(new { success = false, message = "Update failed" });
        }

        return Json(new { success = true, message = "User updated successfully" });
    }
}
