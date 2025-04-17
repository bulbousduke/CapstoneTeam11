using CapstoneTeam11.Models;
using CapstoneTeam11.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CapstoneTeam11.Controllers;
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
    public IActionResult CreateTicket(IFormCollection form)
    {
        var admin = new UserModel()
        {
            AccessLevel = AccessLevel.User,
            Email = "bburger@gmail.com",
            Password = "burger123",
            Name = "Bob Burger"
        };

        var ticket = new TicketModel()
        {
            Category = Category.Hardware,
            CreatedDate = DateTime.Now,
            CreatedBy = admin,
            IsCompleted = false,
            Description = "Charging port not working for laptop",
            Priority = Priority.Medium
        };

        // var note = form["JournalNotes"];
        // if (!string.IsNullOrWhiteSpace(note))
        // {
        //     ticket.JournalNotes.Add(note);
        // }

        _ticketService.Create(ticket);
        return RedirectToAction("Manage");
    }

    [HttpGet]
    public IActionResult Manage(ObjectId id)
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