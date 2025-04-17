using CapstoneTeam11.Models;
using CapstoneTeam11.Services;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneTeam11.Controllers
{
    public class TicketsController : Controller
    {
        private readonly MongoTicketService _ticketService;

        public TicketsController(MongoTicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Shows the form
        }

        [HttpPost]
        public IActionResult Create(IFormCollection form)
        {
            var ticket = new TicketModel
            {
                Title = form["Title"],
                Description = form["Description"],
                Status = "Open",
                CreatedAt = DateTime.UtcNow,
                JournalNotes = new List<string>()
            };

            var note = form["JournalNotes"];
            if (!string.IsNullOrWhiteSpace(note))
            {
                ticket.JournalNotes.Add(note);
            }

            _ticketService.Create(ticket);
            return RedirectToAction("Manage");
        }

        [HttpGet]
public IActionResult Manage(string id)
{
    if (string.IsNullOrEmpty(id))
    {
        // Show list of tickets to choose from
        var allTickets = _ticketService.GetAllTickets();
        return View("ViewPast", allTickets);
    }

    var ticket = _ticketService.GetTicketById(id);
    if (ticket == null)
    {
        return NotFound();
    }

    return View(ticket); // This shows the edit/manage form
}

[HttpPost]
public IActionResult Manage(string id, IFormCollection form)
{
    var updatedTicket = new TicketModel
    {
        Id = id,
        Title = form["Title"],
        Description = form["Description"],
        Status = form["Status"],
        CreatedAt = DateTime.UtcNow, // Optional: get original timestamp if desired
        JournalNotes = new List<string> { form["JournalNotes"] }
    };

    _ticketService.UpdateTicket(id, updatedTicket);
    return RedirectToAction("Manage"); // Go back to list
}

        public IActionResult ViewPast()
        {
            var tickets = _ticketService.GetAllTickets();
            return View(tickets);
        }
    }
}