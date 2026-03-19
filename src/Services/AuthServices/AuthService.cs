using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using src.Common;
using src.DTO.AuthDto;
using src.Services.JwtServices;
using Microsoft.AspNetCore.Identity;
using src.DTO.UserDto;
using src.Models;
using src.Exceptions;
using src.Services.UserClaimServices;
using src.Services.UserLoginHistoryServices;
using src.Services.UserTokenServices;

namespace src.Services.AuthServices;

public class AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
        IJwtService jwtService,
        IUserTokenService userTokenService,
        IHttpContextAccessor httpContextAccessor,
        IUserLoginHistoryService  userLoginHistoryService,
        IUserClaimService userClaimService)
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
        
        var userToken = new IdentityUserToken<string>()
        {
            UserId = user.Id,              // The logged-in user's Id from AspNetUsers
            LoginProvider = "Local",       // "Local" for email/password logins
            Name = "Token",         // Token type
            Value = tokenResponseString // Or your generated secure token
        };
        await userTokenService.CreateAsync(userToken);
        return new ApiResponse<TokenResponseDto>(responseDto, "User registered successfully.");
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto)
    {
        try
        {
            var user = dto.UsernameOrEmail.Contains('@')
                ? await userManager.FindByEmailAsync(dto.UsernameOrEmail)
                : await userManager.FindByNameAsync(dto.UsernameOrEmail);

            if (user == null)
            {
                throw new Exception("User could not be registered.");
            }

            // throw new AppException("Invalid credentials");

            var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded) {
                throw new Exception("Invalid credentials.");
            }
       
            var token = await jwtService.GenerateTokenAsync(user);
            var res = new LoginResponseDto()
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(60),
            };
            
            var loginHistory = new UserLoginHistory()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                LoginTime = DateTime.Now,
                User = user,
                IpAddress = "127.0.0.1",
                UserAgent = "Email/Password"
            };
            await userLoginHistoryService.CreateAsync(loginHistory);
            return new ApiResponse<LoginResponseDto>( res,"User logged in Successfully.");
        }
        catch(Exception ex)
        {
            var res = new LoginResponseDto()
            {
                Token = null!,
                Expiration = DateTime.UtcNow.AddMinutes(0),
            };
            return new ApiResponse<LoginResponseDto>(
                res,
                $"Login failed: {ex.Message}"
            );
        }
        
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


