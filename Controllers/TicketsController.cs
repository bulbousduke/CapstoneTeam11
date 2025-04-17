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

        public IActionResult Manage()
        {
            return View();
        }

        public IActionResult ViewPast()
        {
            var tickets = _ticketService.GetAllTickets();
            return View(tickets);
        }
    }
}