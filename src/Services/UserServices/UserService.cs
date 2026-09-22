using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using src.DBConnection;
using src.DTO.UserDto;
using src.Enums;
using src.Models;
using src.Repositories.UserRepositories;

namespace src.Services.UserServices;

public class UserService(
    IUserRepository userRepository,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext context,
    SignInManager<ApplicationUser> signInManager,
    IWebHostEnvironment environment)
    : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _context = context;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IWebHostEnvironment _environment = environment;


    // ============================================================
    // GET ALL USERS
    // ============================================================

    public async Task<List<UserResponseDto>> GetAllIncludeAsync()
    {
        var users = userManager.Users.ToList();

        var result = new List<UserResponseDto>();

        foreach (var user in users)
        {
            var roleByUsers =
                await userManager.GetRolesAsync(user);

            var permissions = new List<string>();

            foreach (var roleName in roleByUsers)
            {
                var role =
                    await roleManager.FindByNameAsync(roleName);

                if (role != null)
                {
                    var rolePermissions =
                        await context.Set<IdentityRoleClaim<string>>()
                            .Where(r =>
                                r.RoleId == role.Id &&
                                r.ClaimType == "Permission")
                            .Select(r => r.ClaimValue)
                            .ToListAsync();

                    permissions.AddRange(rolePermissions!);
                }
            }

            permissions =
                permissions.Distinct().ToList();

            var userStatus =
                user.Status == UserStatus.Active
                    ? "Active"
                    : user.Status == UserStatus.InActive
                        ? "InActive"
                        : "Suspended";

            result.Add(new UserResponseDto(
                Id: user.Id,
                FullName: user.FullName,
                Email: user.Email!,
                Role: string.Join(", ", roleByUsers),
                Status: userStatus,
                Permission: permissions,
                JoinDate: DateOnly.FromDateTime(user.CreatedAt),
                LastActive: TimeOnly.FromTimeSpan(
                    TimeSpan.Zero)
            ));
        }

        return result;
    }


    // ============================================================
    // ADD ROLE PERMISSION USER
    // ============================================================

    public async Task AddRolePermissionUserAsync(
        UserRequestDto userRolePermissionRequestDto)
    {
        try
        {
            var message = "Success";

            var userName =
                await userManager.FindByNameAsync(
                    userRolePermissionRequestDto.FullName);

            if (userName == null)
                message = "Full Name Not Found";

            var user =
                await userManager.FindByEmailAsync(
                    userRolePermissionRequestDto.Email);

            if (user == null)
                message = "Email Not Found";

            var role =
                await userManager.AddToRoleAsync(
                    user,
                    userRolePermissionRequestDto.Role);

            if (!role.Succeeded)
                message = "Can't add new role to this user";

            user.Status =
                userRolePermissionRequestDto.Status switch
                {
                    "Active" => UserStatus.Active,
                    "InActive" => UserStatus.InActive,
                    _ => UserStatus.Suspended
                };

            await userManager.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    // ============================================================
    // UPDATE USER
    // ============================================================

    public async Task<bool> UpdateUser(
        UserRequestUpdateDto userRequest)
    {
        var user =
            await userManager.FindByIdAsync(
                userRequest.Id);

        if (user!.FullName == "admin@localhost.com")
            return (false);

        if (user == null)
            return false;

        var guid = Guid.NewGuid();

        user.FullName =
            userRequest.FullName;

        user.Email =
            userRequest.Email;

        user.UserName =
            userRequest.Email;

        user.EmailConfirmed = true;

        user.Status =
            userRequest.Status switch
            {
                "Active" => UserStatus.Active,
                "InActive" => UserStatus.InActive,
                _ => UserStatus.Suspended
            };

        user.SecurityStamp =
            guid.ToString();

        var result =
            await userManager.UpdateAsync(user);

        var currentRoles =
            await userManager.GetRolesAsync(user);

        await userManager.RemoveFromRolesAsync(
            user,
            currentRoles);

        await userManager.AddToRoleAsync(
            user,
            userRequest.Role);

        if (!result.Succeeded)
            return false;

        return true;
    }


    // ============================================================
    // CREATE USER
    // ============================================================

    public async Task<(bool, string)> CreateUserAsync(
        UserRequestDto userRequest)
    {
        var user =
            userManager.FindByEmailAsync(
                userRequest.Email);

        if (user == null)
            return (false, "the email already register");

        var Status =
            userRequest.Status switch
            {
                "Active" => UserStatus.Active,
                "InActive" => UserStatus.InActive,
                _ => UserStatus.Suspended
            };

        var newUser =
            new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                FullName = userRequest.FullName,
                Email = userRequest.Email,
                UserName = userRequest.Email,
                Status = Status,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

        var createResult =
            await userManager.CreateAsync(
                newUser,
                "P@ssw0rd123!@#");

        if (!createResult.Succeeded)
            return (false, "Create failed");

        var addRoleResult =
            await userManager.AddToRoleAsync(
                newUser,
                userRequest.Role);

        if (!addRoleResult.Succeeded)
            return (false, "Create failed");

        return (true, "User Created");
    }


    // ============================================================
    // DELETE USER
    // ============================================================

    public async Task<(bool, string)> DeleteUserAsync(
        string id)
    {

        var user =
            await userManager.FindByIdAsync(id);

        if(user!.FullName == "admin@localhost.com")
            return (false, "Can't delete admin");

        if (user == null)
            return (false, "User Not Found");

        var currentRoles =
            await userManager.GetRolesAsync(user);

        var removeRoleResult =
            await userManager.RemoveFromRolesAsync(
                user,
                currentRoles);

        if (!removeRoleResult.Succeeded)
            return (false, "Remove role failed");

        var result =
            await userManager.DeleteAsync(user);

        if (!result.Succeeded)
            return (false, "delete user failed");

        return (true, "User deleted");
    }


    // ============================================================
    // GET PROFILE
    // ============================================================

    public async Task<(bool, string, ProfileUserResponseDto)>
        GetUserProfile(string userId)
    {
        var user =
            await userManager.FindByIdAsync(userId);

        if (user == null)
            return (
                false,
                "User Not Found",
                null
            );

        var currentRoleNames =
            await userManager.GetRolesAsync(user);

        var currentRoles =
            await roleManager.Roles
                .Where(r =>
                    currentRoleNames.Contains(r.Name!))
                .ToListAsync();


        var userProfile =
            new ProfileUserResponseDto(
                user.Id,

                user.FullName,

                // Profile image path
                user.PhotoProfile ?? "",

                user.PhoneNumber ?? "",

                user.Email ?? "",

                string.Join(
                    ",",
                    currentRoles.Select(r => r.Name)
                ),

                string.Join(
                    ",",
                    currentRoles.Select(r => r.Description)
                ),

                user.Status,

                "Null",

                "Null",

                TimeOnly.MaxValue,

                user.CreatedAt
            );


        return (
            true,
            "User listing data",
            userProfile
        );
    }


    // ============================================================
    // UPDATE PROFILE
    // ============================================================

    public async Task<(bool, string)> UpdateUserProfile(
        string userId,
        ProfileUserRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return (
                false,
                "Invalid user id"
            );


        var user =
            await userManager.FindByIdAsync(userId);

        if (user is null)
            return (
                false,
                "User not found"
            );


        // --------------------------------------------------------
        // Full Name
        // --------------------------------------------------------

        if (!string.IsNullOrWhiteSpace(
            request.FullName))
        {
            user.FullName =
                request.FullName.Trim();
        }


        // --------------------------------------------------------
        // Phone
        // --------------------------------------------------------

        if (!string.IsNullOrWhiteSpace(
            request.PhoneNumber))
        {
            user.PhoneNumber =
                request.PhoneNumber.Trim();
        }


        // --------------------------------------------------------
        // Email
        // --------------------------------------------------------

        if (!string.IsNullOrWhiteSpace(
            request.Email) &&
            !string.Equals(
                user.Email,
                request.Email,
                StringComparison.OrdinalIgnoreCase))
        {
            var emailResult =
                await userManager.SetEmailAsync(
                    user,
                    request.Email.Trim());

            if (!emailResult.Succeeded)
            {
                var errors =
                    string.Join(
                        ", ",
                        emailResult.Errors.Select(
                            e => e.Description));

                return (
                    false,
                    errors
                );
            }

            user.UserName =
                request.Email.Trim();
        }


        // --------------------------------------------------------
        // PROFILE PHOTO
        // --------------------------------------------------------

        if (request.PhotoProfile != null &&
            request.PhotoProfile.Length > 0)
        {
            const long maxFileSize =
                5 * 1024 * 1024;


            if (request.PhotoProfile.Length >
                maxFileSize)
            {
                return (
                    false,
                    "Profile image must be smaller than 5 MB."
                );
            }


            var allowedExtensions =
                new[]
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".webp"
                };


            var extension =
                Path.GetExtension(
                    request.PhotoProfile.FileName)
                    .ToLowerInvariant();


            if (!allowedExtensions.Contains(
                extension))
            {
                return (
                    false,
                    "Only JPG, JPEG, PNG and WEBP images are allowed."
                );
            }


            // ----------------------------------------------------
            // Upload directory
            // ----------------------------------------------------

            var uploadDirectory =
                Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "profiles");


            if (!Directory.Exists(
                uploadDirectory))
            {
                Directory.CreateDirectory(
                    uploadDirectory);
            }


            // ----------------------------------------------------
            // Delete old photo
            // ----------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                user.PhotoProfile))
            {
                var oldFileName =
                    Path.GetFileName(
                        user.PhotoProfile);

                var oldFilePath =
                    Path.Combine(
                        uploadDirectory,
                        oldFileName);

                if (System.IO.File.Exists(
                    oldFilePath))
                {
                    System.IO.File.Delete(
                        oldFilePath);
                }
            }


            // ----------------------------------------------------
            // Generate new file name
            // ----------------------------------------------------

            var newFileName =
                $"{Guid.NewGuid()}{extension}";


            var newFilePath =
                Path.Combine(
                    uploadDirectory,
                    newFileName);


            // ----------------------------------------------------
            // Save image
            // ----------------------------------------------------

            await using (
                var stream =
                    new FileStream(
                        newFilePath,
                        FileMode.Create))
            {
                await request.PhotoProfile
                    .CopyToAsync(stream);
            }


            // ----------------------------------------------------
            // Save relative path in database
            // ----------------------------------------------------

            user.PhotoProfile =
                $"/uploads/profiles/{newFileName}";
        }


        // --------------------------------------------------------
        // Save user
        // --------------------------------------------------------

        var updateResult =
            await userManager.UpdateAsync(user);


        if (!updateResult.Succeeded)
        {
            var errors =
                string.Join(
                    ", ",
                    updateResult.Errors.Select(
                        e => e.Description));

            return (
                false,
                errors
            );
        }


        return (
            true,
            "Profile updated successfully"
        );
    }


    // ============================================================
    // REMOVE PROFILE PHOTO
    // ============================================================

    public async Task<(bool, string)> RemoveProfilePhoto(
        string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return (
                false,
                "Invalid user id"
            );


        var user =
            await userManager.FindByIdAsync(userId);

        if (user == null)
            return (
                false,
                "User not found"
            );


        // --------------------------------------------------------
        // Delete physical file
        // --------------------------------------------------------

        if (!string.IsNullOrWhiteSpace(
            user.PhotoProfile))
        {
            var uploadDirectory =
                Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "profiles");


            var fileName =
                Path.GetFileName(
                    user.PhotoProfile);


            var filePath =
                Path.Combine(
                    uploadDirectory,
                    fileName);


            if (System.IO.File.Exists(
                filePath))
            {
                System.IO.File.Delete(
                    filePath);
            }
        }


        // --------------------------------------------------------
        // Remove database path
        // --------------------------------------------------------

        user.PhotoProfile = null;


        var result =
            await userManager.UpdateAsync(user);


        if (!result.Succeeded)
        {
            return (
                false,
                "Unable to remove profile photo"
            );
        }


        return (
            true,
            "Profile photo removed successfully"
        );
    }


    // ============================================================
    // GET NOTIFICATION SETTINGS
    // ============================================================

    public async Task<
        (bool, string, NotificationSettingsDto?)>
        GetNotificationSettings(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return (
                false,
                "Invalid user id",
                null
            );

        var user =
            await userManager.FindByIdAsync(userId);

        if (user is null)
            return (
                false,
                "User not found",
                null
            );

        return (
            true,
            "Notification settings retrieved",
            new NotificationSettingsDto
            {
                SecurityAlertsEnabled = true
            }
        );
    }


    // ============================================================
    // UPDATE NOTIFICATION SETTINGS
    // ============================================================

    public async Task<(bool, string)>
        UpdateNotificationSettings(
            string userId,
            NotificationSettingsDto request)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return (
                false,
                "Invalid user id"
            );

        var user =
            await userManager.FindByIdAsync(userId);

        if (user is null)
            return (
                false,
                "User not found"
            );

        var result =
            await userManager.UpdateAsync(user);

        return result.Succeeded
            ? (
                true,
                "Notification preferences saved successfully"
            )
            : (
                false,
                "Unable to save notification preferences"
            );
    }
}