using src;
using src.Middlewares;

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

    // Skip auth check for allowed paths
    // if (!allowedPaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
    // {
    //     var token = context.Request.Cookies["AuthToken"];
    //     if (string.IsNullOrEmpty(token))
    //     {
    //         // Redirect to login page
    //         // context.Response.Redirect("/Home/Index");
    //         return; // Important: stop further processing
    //     }
    //
    //     // If token exists, you can optionally set it in context.Items for later use
    //     context.Items["AuthToken"] = token;
    // }

    await next();
});

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 401 || response.StatusCode == 403)
    {
        // 1. Ensure we return JSON, not HTML
        response.ContentType = "application/json";

        // 2. Create a standard error response matching your ApiResponse format
        var errorMessage = response.StatusCode == 401
            ? "Unauthorized: You must log in to access this."
            : "Forbidden: You do not have permission to perform this action.";

        // You can format this to match your exact ApiResponse<T> class
        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(new
        {
            success = false,
            message = errorMessage
        });

        await response.WriteAsync(jsonResponse);
    }
});

//  Add authentication before authorization

// Authentication MUST be before Authorization
app.UseAuthentication();
app.UseAuthorization();

// MapControllers MUST be after Authorization

// app.UseMiddleware<ActivityLoggingMiddleware>();

// In Program.cs or Startup.cs (depending on .NET version)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();