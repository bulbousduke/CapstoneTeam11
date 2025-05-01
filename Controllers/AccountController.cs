using CapstoneTeam11.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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
    public IActionResult Register(string name, string email, string password)
    {
        if (_userService.Register(name, email, password, out var error))
        {
            return RedirectToAction("Login");
        }

        ViewBag.Error = error;
        return View();
    }

    // GET: /Account/Login
    public IActionResult Login(string email, string password)
    {
        var user = _userService.Login(email, password);

        if (user != null)
        {
            HttpContext.Session.SetString("Email", user.Email);
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Invalid email or password.";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Logic");
    }
}