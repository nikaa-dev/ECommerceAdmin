using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using src.Models;
using src.Services.UserLoginHistoryServices;

namespace src.Middlewares;

public class ActivityLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ActivityLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserLoginHistoryService auditService)
    {
        // 1. Start a timer to measure performance
        var stopwatch = Stopwatch.StartNew();

        // 2. Enable buffering so we can read the body and then let the controller read it again
        context.Request.EnableBuffering();

        // 3. Read Body (Payload) - BUT skip for login/sensitive routes
        var requestBody = "";
        if (ShouldLogBody(context.Request.Path))
        {
            requestBody = await ReadRequestBody(context.Request);
        }

        // 4. Extract User Info
        var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = context.User?.FindFirst(ClaimTypes.Name)?.Value;
        var ip = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers["User-Agent"].ToString();

        // 5. Execute the actual request
        await _next(context);

        // 6. Stop timer
        stopwatch.Stop();

        // 7. Log only API calls
        if (context.Request.Path.Value?.StartsWith("/api") == true)
        {
            var statusCode = context.Response.StatusCode;
            
            // Format: "POST /api/products/1 - 200 OK (150ms)"
            var actionDescription = $"{context.Request.Method} {context.Request.Path} - Status: {statusCode} ({stopwatch.ElapsedMilliseconds}ms)";
            
            // Add the Payload to your Log method (Update your Service Interface to accept this)
            var userLogin = new UserLoginHistory()
            {
                Action = actionDescription,
                UserId = userId,
                UserName = userName,
                IpAddress = ip,
                Details = requestBody,
                UserAgent = userAgent,
            };
            
            await auditService.CreateAsync(userLogin);
        }
    }

    private async Task<string> ReadRequestBody(HttpRequest request)
    {
        // Ensure the stream is at the beginning
        request.Body.Position = 0;

        using var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        var body = await reader.ReadToEndAsync();

        // IMPORTANT: Reset the stream position so the Controller can read it later
        request.Body.Position = 0;

        return body;
    }

    private bool ShouldLogBody(PathString path)
    {
        var p = path.Value?.ToLower();
        // Don't log passwords!
        if (p != null && (p.Contains("login") || p.Contains("register") || p.Contains("password")))
        {
            return false;
        }
        return true;
    }
}