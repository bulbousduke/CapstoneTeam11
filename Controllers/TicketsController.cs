using CapstoneTeam11.Models;
using CapstoneTeam11.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CapstoneTeam11.Controllers;
public class TicketsController(ITicketService ticketService, IUserService userService) : Controller
{
    private readonly ITicketService _ticketService = ticketService;
    private readonly IUserService _userService = userService;

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(IFormCollection form)
    {
        var ticket = new Ticket
        {
            Description = form["Description"]!,
            Category = Enum.TryParse<Category>(form["Category"], out var category) ? category : Category.Other,
            Priority = Enum.TryParse<Priority>(form["Priority"], out var priority) ? priority : Priority.Low,
            CreatedDate = DateTime.UtcNow,
            IsCompleted = false,
            Assignee = null,
            JournalNotes = []
        };

        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return Unauthorized();

        var user = await _userService.GetUserByEmail(email);
        if (user == null || string.IsNullOrEmpty(user.Id)) return Unauthorized();

        ticket.CreatedBy = user;

        await _ticketService.Create(ticket);
        return RedirectToAction("ViewPast");
    }

    [HttpGet]
    public async Task<IActionResult> ViewPast()
    {
        var allTickets = await _ticketService.GetAllTickets();
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var userAccessLevel = User.FindFirst("AccessLevel")?.Value;

        if (userEmail == null || userAccessLevel == null)
            return Unauthorized();

        IEnumerable<Ticket> visibleTickets;

        if (userAccessLevel == "Admin")
        {
            visibleTickets = allTickets;
        }
        else if (userAccessLevel == "Employee")
        {
            var employee = await _userService.GetUserByEmail(userEmail);
            var allowedCategories = employee!.AssignedCategories
                .Select(c => Enum.TryParse<Category>(c, out var cat) ? cat : Category.Other)
                .ToList();

            visibleTickets = [.. allTickets.Where(t => allowedCategories.Contains(t.Category))];
        }
        else
        {
            var user = await _userService.GetUserByEmail(userEmail);
            visibleTickets = allTickets.Where(t => t.CreatedBy?.Id == user!.Id);
        }

        var sortedTickets = visibleTickets
            .OrderBy(t => t.IsCompleted) // false (not completed) first
            .ThenBy(t => t.Priority == Priority.High ? 0 :
                        t.Priority == Priority.Medium ? 1 : 2) // custom priority order
            .ToList();

        return View(sortedTickets);
    }

    [HttpGet]
    public async Task<IActionResult> EditTicket(string id)
    {
        if (string.IsNullOrEmpty(id)) return NotFound();
        var ticket = await _ticketService.GetTicketById(id);
        return ticket == null ? NotFound() : View("Edit", ticket);
    }

    [HttpPost]
    public async Task<IActionResult> EditTicket(string id, IFormCollection form)
    {
        if (string.IsNullOrEmpty(id)) return NotFound();

        var ticketToUpdate = await _ticketService.GetTicketById(id);
        if (ticketToUpdate == null) return NotFound();

        try
        {
            ticketToUpdate.Description = form["Description"]!;

            if (Enum.TryParse(form["Category"], out Category parsedCategory))
                ticketToUpdate.Category = parsedCategory;

            ticketToUpdate.Priority = Enum.TryParse<Priority>(form["Priority"], out var parsedPriority) ? parsedPriority : ticketToUpdate.Priority;
            ticketToUpdate.IsCompleted = form["IsCompleted"].FirstOrDefault() == "true";
            ticketToUpdate.Assignee = form["Assignee"];

            await _ticketService.Update(id, ticketToUpdate);
            return RedirectToAction("ViewPast");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error updating ticket: {ex.Message}");
            return View("Edit", ticketToUpdate);
        }
    }

    // ROUTE: /Tickets/Manage — Shows dashboard/statistics
    [HttpGet]
    public IActionResult Manage()
    {
        ViewBag.TotalTickets = _ticketService.GetTotalTickets();
        var (open, closed) = _ticketService.GetOpenClosedTicketCounts();
        ViewBag.OpenTickets = open;
        ViewBag.ClosedTickets = closed;
        ViewBag.TicketsByCategory = _ticketService.GetTicketsByCategory();

        ViewBag.TotalUsers = _userService.GetTotalUsers();
        ViewBag.UsersByRole = _userService.GetUsersByAccessLevel();

        return View("Manage"); // Looks for Views/Tickets/Manage.cshtml
    }
}