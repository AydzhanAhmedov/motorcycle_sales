using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace motorcycle_sales.Core.Authorization;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
        // doesn't handle all policies it should fall back to an alternate provider.
        BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }



    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        // If policy name starts with Permission then we are usin PermissionRequirement
        if (policyName.StartsWith("Permission", StringComparison.OrdinalIgnoreCase))
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionRequirement(policyName));
            return Task.FromResult(policy.Build());
        }

        return BackupPolicyProvider.GetPolicyAsync(policyName);
    }

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() =>
        Task.FromResult<AuthorizationPolicy>(null);

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
        Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
}
