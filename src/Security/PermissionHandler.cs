using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using src.Services.UserServices;

namespace src.Security;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PermissionHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        Console.WriteLine($"\n--- [AUTH DEBUG] Checking Permission: '{requirement.Permission}' ---");

        // 1. Try to find the User ID claim
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) ??
                          context.User.FindFirst("sub") ??
                          context.User.FindFirst("id");

        if (userIdClaim == null)
        {
            Console.WriteLine("[AUTH DEBUG] ❌ FAILED: Could not find User ID in the token.");
            Console.WriteLine("[AUTH DEBUG] Available claims in token:");
            foreach (var claim in context.User.Claims)
            {
                Console.WriteLine($"   - {claim.Type}: {claim.Value}");
            }
            return;
        }

        var userId = userIdClaim.Value;
        Console.WriteLine($"[AUTH DEBUG] ✅ User ID found: {userId}");

        // 2. Check Database
        using var scope = _scopeFactory.CreateScope();
        var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        bool hasPermission = await permissionService.HasPermissionAsync(userId, requirement.Permission);

        Console.WriteLine($"[AUTH DEBUG] Database check returned: {hasPermission}");

        if (hasPermission)
        {
            Console.WriteLine("[AUTH DEBUG] ✅ SUCCESS: Permission granted.");
            context.Succeed(requirement);
        }
        else
        {
            Console.WriteLine("[AUTH DEBUG] ❌ FAILED: Database returned false. User does not have this permission.");
        }
        Console.WriteLine("--------------------------------------------------\n");
    }
}