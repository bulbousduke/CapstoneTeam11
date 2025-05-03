using CapstoneTeam11.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace CapstoneTeam11.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: /Account/Register
    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(string name, string email, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            ViewBag.Error = "Passwords do not match.";
            return View();
        }

        if (_userService.Register(name, email, password, out var error))
        {
            return RedirectToAction("Login");
        }

        ViewBag.Error = error;
        return View();
    }

    // GET: /Account/Login
    [AllowAnonymous]
    public IActionResult Login()
    {
        var emailCookie = Request.Cookies["PreviousEmails"];
        ViewBag.PreviousEmails = emailCookie?.Split(',').Distinct().ToList() ?? new List<string>();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = _userService.Login(email, password);
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email),
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Invalid email or password.";
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToAction("Login");
    }
}