using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CapstoneTeam11.Services;
using CapstoneTeam11.Models;

namespace CapstoneTeam11.Controllers;
[Authorize]
public class HomeController(ITicketService ticketService, IUserService userService) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly ITicketService _ticketService = ticketService;

    public async Task<IActionResult> Index()
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var accessLevel = User.FindFirst("AccessLevel")?.Value;

        if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(accessLevel))
            return RedirectToAction("Login", "Account");

        var allTickets = await _ticketService.GetAllTickets();
        var user = await _userService.GetUserByEmail(userEmail);

        List<Ticket> ticketsToShow;

        switch (accessLevel)
        {
            case "Admin":
                ticketsToShow = allTickets; // Admins see all
                break;

            case "Employee":
                var categories = user!.AssignedCategories
                    .Select(c => Enum.TryParse<Category>(c, out var cat) ? cat : Category.Other)
                    .ToList();

                ticketsToShow = [.. allTickets.Where(t => t.Assignee == user.Id && categories.Contains(t.Category))];
                break;

            default: 
                ticketsToShow = [.. allTickets.Where(t => t.CreatedBy?.Id == user!.Id)];
                break;
        }

        ViewBag.AccessLevel = accessLevel;
        return View(ticketsToShow);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ManageUsers()
    {
        var users = await _userService.GetAllUsers();
        return View(users);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserRole(string userId, AccessLevel newRole)
    {
        var user = await _userService.GetUserById(userId);
        if (user == null) return NotFound();

        user.AccessLevel = newRole;
        await _userService.Update(userId, user);

        return RedirectToAction("ManageUsers");
    }

    [HttpGet]
    public IActionResult Help()
    {
        return View();
    }
}