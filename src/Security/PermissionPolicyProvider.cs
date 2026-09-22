using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace src.Security;

public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) { }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null)
        {
            return policy;
        }

        // Create the policy dynamically and bind it specifically to JWT
        var policyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
        policyBuilder.AddRequirements(new PermissionRequirement(policyName));

        return policyBuilder.Build();
    }
}