using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace motorcycle_sales.Core.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    public PermissionAuthorizationHandler()    { }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User == null)
        {
            return;
        }

        var permissionss = context.User.Claims.Where(x => x.Type == "Permission" && x.Value == requirement.Permission);
        
        if (permissionss.Any())
        {
            context.Succeed(requirement);
            return;
        }
    }

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        base.HandleAsync(context);

        return Task.CompletedTask;
    }
}
