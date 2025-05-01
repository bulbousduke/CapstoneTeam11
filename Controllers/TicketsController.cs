using System.Threading.Tasks;
using BCrypt.Net;
using CapstoneTeam11.Models;
using CapstoneTeam11.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CapstoneTeam11.Controllers;
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
        return View(); // Shows the form
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket(IFormCollection form)
    {
        var admin = new User()
        {
            AccessLevel = AccessLevel.User,
            Email = "bburger@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("burger123"),
            Name = "Bob Burger"
        };

        var ticket = new Ticket()
        {
            Category = Category.Hardware,
            CreatedDate = DateTime.Now,
            CreatedBy = admin,
            IsCompleted = false,
            Description = "Charging port not working for laptop",
            Priority = Priority.Medium
        };

        await _ticketService.Create(ticket);
        return RedirectToAction("Manage");
    }

    [HttpGet]
    public IActionResult Manage(string id)
    {
        var ticket = _ticketService.GetTicketById(id);
        if (ticket == null)
        {
            return NotFound();
        }

        return View(ticket); // This shows the edit/manage form
    }

    // [HttpPost]
    // public IActionResult Manage(ObjectId id, IFormCollection form)
    // {
    //     // var updatedTicket = new TicketModel
    //     // {
    //     //     Title = form["Title"],
    //     //     Description = form["Description"],
    //     //     Status = form["Status"],
    //     //     CreatedDate = DateTime.UtcNow, // Optional: get original timestamp if desired
    //     //     JournalNotes = new List<string> { form["JournalNotes"] }
    //     // };

    //     // _ticketService.UpdateTicket(id, updatedTicket);
    //     // return RedirectToAction("Manage"); // Go back to list
    // }

    public IActionResult ViewPast()
    {
        var tickets = _ticketService.GetAllTickets();
        return View(tickets);
    }
}