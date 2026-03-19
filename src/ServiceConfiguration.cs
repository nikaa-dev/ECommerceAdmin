using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using src.Repositories.UserRepositories;
using src.Repositories.RoleRepositories;
using src.Services.UserServices;
using src.Services.RoleServices;
using src.Repositories;
using src.DBConnection;
using src.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using src.Common.Configs;
using src.Services.AuthServices;
using src.Services.JwtServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using src.Auth;
using src.Repositories.UserClaimRepositories;
using src.Repositories.UserTokenRepositories;
using src.Services.UserClaimServices;
using src.Services.UserTokenServices;

namespace src;

public static class ServiceConfiguration
{
    public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Correct: use the parameter 'services', not 'Services'
        services.AddControllersWithViews();
        services.AddHttpContextAccessor(); 
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserTokenRepository,UserTokenRepository>();
        services.AddScoped<IUserClaimRepository, UserClaimRepository>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserClaimService, UserClaimService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserTokenService, UserTokenService>();
        
        var jwtSettings = configuration.GetSection("JWT").Get<JwtConfig>();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Secret!));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => 
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Read token from cookie
                        var token = context.HttpContext.Request.Cookies["AuthToken"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings!.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),
                    RoleClaimType = ClaimTypes.Role
                };
            });
        services.Configure<JwtConfig>(configuration.GetSection("JWT"));
        services.AddAuthorization();
    }
}