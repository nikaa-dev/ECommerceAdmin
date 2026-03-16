using System.Text;
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
namespace src;

public static class ServiceConfiguration
{
    public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Correct: use the parameter 'services', not 'Services'
        services.AddControllersWithViews();
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        
        
        var jwtSettings = configuration.GetSection("JWT").Get<JwtConfig>();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Secret!));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings!.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });
        services.Configure<JwtConfig>(configuration.GetSection("JWT"));

        services.AddAuthorization();
    }
}