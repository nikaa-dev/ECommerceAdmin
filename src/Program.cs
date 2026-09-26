using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using src;
using src.DBConnection;
using src.Models;
using src.SeedData;

var builder = WebApplication.CreateBuilder(args);

// 1. Register Services
builder.Services.ConfigureApplicationServices(builder.Configuration);

var app = builder.Build();

// ---------------------------------------------------------
// 2. RUN DATABASE MIGRATION & SEEDING FIRST
// ---------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var defaultUserConfig = services.GetRequiredService<IOptions<DefaultUserConfig>>();

        await DbInitializer.SeedAsync(userManager, roleManager, defaultUserConfig);
        Console.WriteLine("✅ Database Check/Seed Completed.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Migration/Seeding Error: {ex.Message}");
    }
}

// ---------------------------------------------------------
// 3. CONFIGURE HTTP PIPELINE (Middleware)
// ---------------------------------------------------------
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// These two lines natively protect your app using the settings from ServiceConfiguration.cs
app.UseAuthentication();
app.UseAuthorization();

// Default Route Setup
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();