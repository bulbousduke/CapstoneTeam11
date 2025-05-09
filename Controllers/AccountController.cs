using CapstoneTeam11.Models;
using CapstoneTeam11.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;


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
        
            public async Task<IActionResult> Register(string name, string email, string password, string confirmPassword)
{
    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) ||
        string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
    {
        ViewBag.Error = "All fields are required.";
        return View();
    }

    if (password != confirmPassword)
    {
        ViewBag.Error = "Passwords do not match.";
        return View();
    }

    if (password.Length < 8 || password.Length > 64)
    {
        ViewBag.Error = "Password must be between 8 and 64 characters.";
        return View();
    }

    if (!Regex.IsMatch(password, @"\d"))
    {
        ViewBag.Error = "Password must include at least one number.";
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
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            var user = _userService.Login(email, password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.Name),
                    new(ClaimTypes.Email, user.Email),
                    new("AccessLevel", user.AccessLevel.ToString())
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", principal, new AuthenticationProperties
                {
                    IsPersistent = rememberMe
                });

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userService.GetAllUsers();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(string userId, AccessLevel newRole)
        {
            bool updated = await _userService.UpdateAccessLevel(userId, newRole);
            if (!updated)
            {
                TempData["Error"] = "Failed to update user role.";
            }
            return RedirectToAction("ManageUsers");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            TempData["LogoutMessage"] = "You have been signed out successfully.";
            return RedirectToAction("Login", "Account");
        }
    }
}