using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using motorcycle_sales.Core.Authorization;
using motorcycle_sales.Core.Entities;

namespace motorcycle_sales.Web.Seed;

public static class SeedUserManagament
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await AddRolesAsync(roleManager);
        await AddSuperAdmin(userManager, roleManager, configuration);
    }

    private static async Task AddSuperAdmin(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        var superAdmin = new ApplicationUser
        {
            UserName = configuration["SuperAdmin:Username"],
            Email = configuration["SuperAdmin:Username"],
            EmailConfirmed = true
        };

        var superAdminExists = await userManager.FindByEmailAsync(superAdmin.Email) != null;
        if (!superAdminExists)
        {
            await userManager.CreateAsync(superAdmin, configuration["SuperAdmin:Password"]);
            await userManager.AddToRoleAsync(superAdmin, Roles.SuperAdmin.ToString());
        }

        // add claims
        var superAdminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());

        var allClaims = await roleManager.GetClaimsAsync(superAdminRole);
        var allPermissions = Permissions.GetAllPermisions();
        foreach (var permission in allPermissions)
        {
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(superAdminRole, new Claim("Permission", permission));
            }
        }
    }

    private static async Task AddRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in Enum.GetValues(typeof(Roles)))
        {
            var roleExists = await roleManager.RoleExistsAsync(role.ToString());
            if (!roleExists)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                if (!result.Succeeded)
                {
                    // log fail
                }
            }
        }
    }
}
