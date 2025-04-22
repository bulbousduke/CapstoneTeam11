using CapstoneTeam11.Services;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneTeam11.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
}