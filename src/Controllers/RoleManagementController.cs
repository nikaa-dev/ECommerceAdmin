using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTO.UserDto;
using src.Enums;
using src.Services.RoleServices;
using src.Services.UserServices;
using src.Extensions.Pagenations;
using src.DTO.RoleDto;

namespace src.Controllers;

public class RoleManagementController(IUserService userService, ILogger<UserManagementController> logger, IRoleService roleService) : Controller
{

    private readonly ILogger<UserManagementController> _logger = logger;
    private readonly IRoleService _roleService = roleService;

    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Index(string? searchItem, int pageNumber = 1)
    {
        var roles = await roleService.GetAllRoleIncludeAsync();
        var users = await userService.GetAllIncludeAsync();

        // lookup permission
        ViewBag.rolePermissionLookup = await roleService.GetLookupRolePermission();


        var status = Enum.GetValues(typeof(UserStatus)).Cast<UserStatus>().ToList();
       

        ViewBag.TotalRoles = roles.Count();
        ViewBag.TotalUsers = users.Count(); 
        ViewBag.TotalHightAccess = roles.Select(role => role.AccessLevel).Count(u => u == "Hight");
        ViewBag.TotalMediumAccess = roles.Select(role => role.AccessLevel).Count(u => u == "Medium");


        if (searchItem != null)
        {
            roles = roles.Where(r =>
                r.RoleName.Contains(searchItem) ||
                r.Description.Contains(searchItem)).ToList();
        }

        var queryable = roles.AsQueryable();
        var rolePagination = queryable.ToPagedResultAsync(pageNumber, 8);

        return View(rolePagination);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoleRequestCreateDto roleRequestCreate)
    {
        try
        {
            var (status, messageStatus) = await roleService.CreateRoleAsync(roleRequestCreate);
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
