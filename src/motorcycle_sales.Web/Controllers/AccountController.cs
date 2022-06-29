using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using motorcycle_sales.Core.Entities;
using motorcycle_sales.Core.Interfaces;
using motorcycle_sales.Web.ViewModels;

namespace motorcycle_sales.Web.Controllers;
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAdvertisementService _advertisementService;
    private readonly IUserSearchFilterService _userSearchFilterService;

    public AccountController(SignInManager<ApplicationUser> signInManager
        , UserManager<ApplicationUser> userManager
        , IAdvertisementService advertisementService
        , IUserSearchFilterService userSearchFilterService)
    {
        this._signInManager = signInManager;
        this._userManager = userManager;
        this._advertisementService = advertisementService;
        this._userSearchFilterService = userSearchFilterService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(loginViewModel);

        var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);

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

        var identityUser = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber
        };

        var result = await _userManager.CreateAsync(identityUser, model.Password);


        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(identityUser, isPersistent: false);
            return RedirectToAction("index", "home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("index", "home");
    }

    [HttpGet]
    public async Task<IActionResult> Profile(string userId)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return NotFound();
        }

        // Check if user is looking at own profile
        if (userId != currentUserId)
        {
            return BadRequest();
        }

        var profileViewModel = new ProfileViewModel();

        profileViewModel.User = await _userManager.GetUserAsync(HttpContext.User);

        return View(profileViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> MyAdvertisements(string userId)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return NotFound();
        }

        // Check if user is looking at own profile
        if (userId != currentUserId)
        {
            return BadRequest();
        }

        var model = (await _advertisementService.GetAdvertisementsByUserId(userId)).Value;

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Filters(string userId)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            return NotFound();
        }

        // Check if user is looking at own profile
        if (userId != currentUserId)
        {
            return BadRequest();
        }

        var model = (await _userSearchFilterService.GetUserSearchFiltersByUserId(userId)).Value;

        return View(model);
    }
}
