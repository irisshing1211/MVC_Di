using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVC_Di.Models;
using MVC_Di.Services;

namespace MVC_Di.Controllers;

public class AccountController(IAuthService authService, ILogger<AccountController> logger) : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Ledger");
        }

        return View(new LoginViewModel());
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Ledger");
        }

        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel input)
    {
        if (!ModelState.IsValid)
        {
            return View(input);
        }

        var user = await authService.ValidateUserAsync(input.Username, input.Password);
        if (user is null)
        {
            input.ErrorMessage = "帳號或密碼錯誤";
            return View(input);
        }

        await SignInUserAsync(user);
        logger.LogInformation("User {Username} signed in", user.Username);

        return RedirectToAction("Index", "Ledger");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel input)
    {
        if (!ModelState.IsValid)
        {
            return View(input);
        }

        if (await authService.UsernameExistsAsync(input.Username))
        {
            input.ErrorMessage = "帳號已被使用";
            return View(input);
        }

        var user = await authService.RegisterUserAsync(input);
        if (user is null)
        {
            input.ErrorMessage = "註冊失敗，請稍後再試";
            return View(input);
        }

        await SignInUserAsync(user);
        logger.LogInformation("User {Username} signed in after registration", user.Username);

        return RedirectToAction("Index", "Ledger");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        logger.LogInformation("User {Username} signed out", User.Identity?.Name);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    private async Task SignInUserAsync(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new("DisplayName", user.DisplayName)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}
