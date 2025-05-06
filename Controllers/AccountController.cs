using CapstoneTeam11.Models;
using CapstoneTeam11.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CapstoneTeam11.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /Account/Register
       [HttpGet]
public IActionResult Register()
{
    return View();
}


[HttpPost]
public async Task<IActionResult> Register(string name, string email, string password)
{
    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
    {
        ViewBag.Error = "All fields are required.";
        return View();
    }

    if (await _userService.GetUserByEmail(email) != null)
    {
        ViewBag.Error = "Email is already registered.";
        return View();
    }

    var newUser = new User
    {
        Name = name,
        Email = email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
        AccessLevel = AccessLevel.User,
        AssignedCategories = new List<string>()
    };

    await _userService.Create(newUser);

    return RedirectToAction("Login");
}

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            var emailCookie = Request.Cookies["PreviousEmails"];
            ViewBag.PreviousEmails = emailCookie?.Split(',').Distinct().ToList() ?? new List<string>();
            return View();
        }

        [HttpPost]
        [HttpPost]
public async Task<IActionResult> Login(string email, string password, bool rememberMe)
{
    var user = _userService.Login(email, password);
    if (user != null)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email),
            new("AccessLevel", user.AccessLevel.ToString()) // Add AccessLevel claim
        };

        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("MyCookieAuth", principal, new AuthenticationProperties
        {
            IsPersistent = rememberMe
        });

        // Redirect based on AccessLevel
        return user.AccessLevel switch
        {
            AccessLevel.Admin => RedirectToAction("AdminDashboard", "Home"),
            AccessLevel.Employee => RedirectToAction("EmployeeDashboard", "Home"),
            _ => RedirectToAction("UserDashboard", "Home")
        };
    }

    ViewBag.Error = "Invalid email or password.";
    return View();
}

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        
    }
}