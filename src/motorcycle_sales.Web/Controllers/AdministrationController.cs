using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using motorcycle_sales.Core.Entities;
using motorcycle_sales.Web.ViewModels;

namespace motorcycle_sales.Web.Controllers;
public class AdministrationController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdministrationController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public IActionResult ListRoles()
    {
        var roles = _roleManager.Roles;

        return View(roles);
    }


    [HttpGet]
    public IActionResult CreateRole()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoleAsync(CreateRoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            IdentityRole identityRole = new IdentityRole()
            {
                Name = model.RoleName
            };

            IdentityResult result = await _roleManager.CreateAsync(identityRole);

            if(result.Succeeded)
            {
                return Json(new { isValid = true, html = Url.Action("ListRoles", "Administration", null, "http") });
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return Json(new { isValid = false, html = this.RenderRazorViewToString("CreateRole", model) });
    }
}
