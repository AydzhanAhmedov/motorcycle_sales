using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using motorcycle_sales.Core.Authorization;
using motorcycle_sales.Core.Entities;
using motorcycle_sales.SharedKernel.Interfaces;
using motorcycle_sales.Web.ViewModels;

namespace motorcycle_sales.Web.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class AdministrationController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public IActionResult ListRoles()
    {
        List<string> systemRoles = Enum.GetValues(typeof(Roles)).Cast<Roles>().Select(role => role.ToString()).ToList();

        List<RoleViewModel> rolesViewModel = new List<RoleViewModel>();

        foreach (var role in _roleManager.Roles)
        {
            rolesViewModel.Add(new RoleViewModel()
            {
                RoleId = role.Id,
                RoleName = role.Name,
                UsersCount = _userManager.GetUsersInRoleAsync(role.Name).Result.Count,
                IsSystemRole = systemRoles.Contains(role.Name)
            });
        }

        var rolesViewModelOrdered = rolesViewModel.OrderByDescending(role => role.IsSystemRole);

        return View(rolesViewModelOrdered);
    }

    [HttpGet]
    public async Task<IActionResult> AddOrEditRoleAsync(string ?RoleId)
    {
        if (String.IsNullOrEmpty(RoleId))
            return View(new AddOrEditRoleViewModel() { RoleId = ""});

        var role = await _roleManager.FindByIdAsync(RoleId);
        if (role == null)
            return NotFound();

        var roleModel = new AddOrEditRoleViewModel()
        {
            RoleId = role.Id,
            RoleName = role.Name
        };

        return View(roleModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrEditRoleAsync(AddOrEditRoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            IdentityResult result = null;

            if (string.IsNullOrEmpty(model.RoleId))
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = model.RoleName
                };

                result = await _roleManager.CreateAsync(identityRole);
            }
            else
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId);
                if (role == null)
                {
                    return Json(new { isValid = true, html = Url.Action("ListRoles", "Administration", null, "http") });
                    // Log fail
                }

                role.Name = model.RoleName;

                result = await _roleManager.UpdateAsync(role);
            }

            if (result.Succeeded)
            {
                return Json(new { isValid = true, html = Url.Action("ListRoles", "Administration", null, "http") });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return Json(new { isValid = false, html = this.RenderRazorViewToString("CreateRole", model) });
    }

    [HttpGet]
    public IActionResult ListUsers()
    {

        var users = _userManager.Users;
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> ManageUserRolesAsync(string userId)
    {
        List<string> systemRoles = Enum.GetValues(typeof(Roles)).Cast<Roles>().Select(role => role.ToString()).ToList();

        var userRoles = new List<ManageUserRolesViewModel.UserRole>();
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound();

        foreach (var role in _roleManager.Roles)
        {
            var useRole = new ManageUserRolesViewModel.UserRole()
            {
                RoleName = role.Name,
                Selected = await _userManager.IsInRoleAsync(user, role.Name)
            };

            userRoles.Add(useRole);
        }

        userRoles = userRoles.OrderByDescending(role => systemRoles.Contains(role.RoleName)).ToList();

        var model = new ManageUserRolesViewModel()
        {
            Username = user.UserName,
            UserId = userId,
            UserRoles = userRoles
        };

        return View(model);
    }

    public async Task<IActionResult> ManageUserRolesAsync(ManageUserRolesViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        var roles = await _userManager.GetRolesAsync(user);
        
        // Delete all user roles
        var result = await _userManager.RemoveFromRolesAsync(user, roles);

        // Add all selcted roles
        result = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));

        return RedirectToAction("ListUsers");
    }

    [HttpGet]
    public async Task<ActionResult> ManageRolePermissions(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            return NotFound();
        }

        var claims = await _roleManager.GetClaimsAsync(role);

        // Load all possible permissions
        var allPermissions = Permissions.GetAllPermisions()
            .Select(permission => new ManageRolePermissionsViewModel.RoleClaim
            {
                Type = "Permission",
                Value = permission,
                Selected = claims.Any(claim => claim.Type == "Permission" && claim.Value == permission)
            }).ToList();

        var model = new ManageRolePermissionsViewModel()
        {
            RoleClaims = allPermissions,
            RoleId = roleId,
            RoleName = role.Name
        };

        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> ManageRolePermissions(ManageRolePermissionsViewModel model)
    {
        var role = await _roleManager.FindByIdAsync(model.RoleId);
        var permissionClaim = (await _roleManager.GetClaimsAsync(role)).Where(claim => claim.Type == "Permission");

        // Delete all permission claims from role
        foreach (var claim in permissionClaim)
        {
            await _roleManager.RemoveClaimAsync(role, claim);
        }

        var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();

        // Add all selected permission claims
        foreach (var claim in selectedClaims)
        {
            await _roleManager.AddClaimAsync(role, new Claim("Permission", claim.Value));
        }

        return RedirectToAction("ListRoles");
    }

    public async Task<IActionResult> DeleteRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            return NotFound();
        }

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {

        }

        return Ok();
    }
}
