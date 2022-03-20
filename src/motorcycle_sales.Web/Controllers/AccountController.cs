using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using motorcycle_sales.Web.ViewModels;

namespace motorcycle_sales.Web.Controllers;
public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly UserManager<IdentityUser> userManager;

    public AccountController(SignInManager<IdentityUser> signInManager
        , UserManager<IdentityUser> userManager)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
    {
        if (!ModelState.IsValid)
            return View(loginViewModel);

        var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Home");  
        }

        ModelState.AddModelError(string.Empty, "Invalid login atempt");
        return View(loginViewModel);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var identityUser = new IdentityUser
        {
            UserName = model.Email,
            Email = model.Email
        };

        var result = await userManager.CreateAsync(identityUser, model.Password);

        if (result.Succeeded)
        {
            await signInManager.SignInAsync(identityUser, isPersistent: false);
            return RedirectToAction("index", "home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }
}
