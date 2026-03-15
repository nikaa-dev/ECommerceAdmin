using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using src.Services;
using src.Models;
using src.Common.Configs;
using src.Services.JwtServices;

namespace src.Services.JwtServices;

public class JwtService : IJwtService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager; // Must use ApplicationRole, not IdentityRole
    private readonly JwtConfig _jwtConfig;

    public JwtService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<JwtConfig> jwtOptions)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtConfig = jwtOptions.Value;
    }

    public async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id), // Standard ID claim
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique Token ID
            new Claim(ClaimTypes.NameIdentifier, user.Id), // Required for User.FindFirst(NameIdentifier)
            new Claim(ClaimTypes.Name, user.UserName ?? "")
        };
        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var roleName in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                var permissions = roleClaims.Where(c => c.Type == "Permission");
                claims.AddRange(permissions);
            }
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpireMinutes),
            Issuer = _jwtConfig.Issuer,
            Audience = _jwtConfig.Audience,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}