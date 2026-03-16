using src.DBConnection;
using Microsoft.EntityFrameworkCore;
using src;

var builder = WebApplication.CreateBuilder(args);

// Register all services
builder.Services.ConfigureApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔑 Add authentication before authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();