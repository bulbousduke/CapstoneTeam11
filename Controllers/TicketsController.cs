using CapstoneTeam11.Models;
using CapstoneTeam11.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson; 

namespace CapstoneTeam11.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketService _ticketService;

        public TicketsController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
public async Task<IActionResult> Create(IFormCollection form)
{
    var ticket = new Ticket
    {
        Description = form["Description"],
        Category = Enum.TryParse<Category>(form["Category"], out var category) ? category : Category.Other,
        Priority = Enum.TryParse<Priority>(form["Priority"], out var priority) ? priority : Priority.Low,
        CreatedDate = DateTime.UtcNow,
        IsCompleted = false,
        CreatedBy = new User
        {
            UserId = ObjectId.GenerateNewId().ToString(),
            AccessLevel = AccessLevel.User,
            Email = "admin@example.com",
            Password = "admin123",
            Name = "Admin User"
        },
        Assignee = null, // You can update this if your form has an assignee field
        JournalNotes = new List<string>()
    };

    await _ticketService.Create(ticket);

    return RedirectToAction("ViewPast");
}

        [HttpGet]
        public async Task<IActionResult> ViewPast()
        {
            var tickets = await _ticketService.GetAllTickets();
            return View(tickets);
        }

      