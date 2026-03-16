using src.Common;
using src.DTO.AuthDto;
using src.DTO;
using src.Services.JwtServices;
using src.Services.AuthServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using src.Models;
using src.Common.Configs;
using src.DBConnection;
using src.DTO.AuthDto;
using src.Exceptions;
using src.Services.UserTokenServices;

namespace src.Services.AuthServices;

public class AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
        IJwtService jwtService,
        IUserTokenService userTokenService)
        : IAuthService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    public async Task<ApiResponse<TokenResponseDto>> RegisterAsync(RegisterDto dto)
    {
        if (await userManager.FindByEmailAsync(dto.Email) != null)
            throw new AppException($"Email '{dto.Email}' is already registered.");

        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email,
            FullName = dto.FullName,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description));

        await userManager.AddToRoleAsync(user, "User");

        var tokenResponseString = await jwtService.GenerateTokenAsync(user);

        var responseDto = new TokenResponseDto
        {
            Token = tokenResponseString,
            Expiration = DateTime.UtcNow.AddMinutes(60) // Ideally match this with your config
        };
        return new ApiResponse<TokenResponseDto>(responseDto, "User registered successfully.");
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = dto.UsernameOrEmail.Contains("@")
            ? await userManager.FindByEmailAsync(dto.UsernameOrEmail)
            : await userManager.FindByNameAsync(dto.UsernameOrEmail);

        if (user == null) throw new AppException("Invalid credentials");

        var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded) throw new AppException("Invalid credentials");

        var token = await jwtService.GenerateTokenAsync(user);
        var userToken = new IdentityUserToken<string>()
        {
            UserId = user.Id,              // The logged-in user's Id from AspNetUsers
            LoginProvider = "Local",       // "Local" for email/password logins
            Name = "Token",         // Token type
            Value = token // Or your generated secure token
        };
        await userTokenService.CreateAsync(userToken);
        
        var roles = await userManager.GetRolesAsync(user);

        // Logic to get permissions... (simplified for brevity)
        var permissions = new List<string>();

        var responseDto = new LoginResponseDto
        {
            Token = token,
            User = new UserDetailDto { Id = user.Id, FullName = user.FullName, Email = user.Email, Roles = roles, Permissions = permissions }
        };

        return new ApiResponse<LoginResponseDto>(responseDto, "Login successful.");
    }

    public async Task<ApiResponse<UserDetailDto>> GetCurrentUserAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) throw new NotFoundException("User", userId);

        return new ApiResponse<UserDetailDto>(new UserDetailDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email
        });
    }
}


