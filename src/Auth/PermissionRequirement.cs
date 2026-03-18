using Microsoft.AspNetCore.Authorization;

namespace src.Auth;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}