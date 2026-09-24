using src;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApplicationServices(builder.Configuration);

var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.Use(async (context, next) =>
{
    // List of paths that do NOT require auth (like login, register, static files)
    var allowedPaths = new[] { "/Account/Login", "/Account/Register", "/css", "/js", "/images" };

    var path = context.Request.Path.Value;

    await next();
});

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 403 || response.StatusCode == 401)
    {
        response.Redirect("/Account/AccessDenied");
    }
    await Task.CompletedTask;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();