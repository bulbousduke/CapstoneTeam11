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
        Assignee = null, 
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

        [HttpGet]
        public async Task<IActionResult> Manage(string id)
        {
            var ticket = await _ticketService.GetTicketById(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

// GET: Tickets/Edit/{id}
[HttpGet]
public async Task<IActionResult> EditTicket(string id)
{
    if (string.IsNullOrEmpty(id))
    {
        return NotFound();
    }

    var ticket = await _ticketService.GetTicketById(id);
    if (ticket == null)
    {
        return NotFound();
    }

    return View("Edit", ticket); // Pass ticket to Edit.cshtml
}

// POST: Tickets/Edit/{id}
[HttpPost]
public async Task<IActionResult> EditTicket(string id, IFormCollection form)
{
    if (string.IsNullOrEmpty(id))
    {
        return NotFound();
    }

    var ticketToUpdate = await _ticketService.GetTicketById(id);
    if (ticketToUpdate == null)
    {
        return NotFound();
    }

    try
    {
        ticketToUpdate.Description = form["Description"];
        
        // Safely parse Category
        if (Enum.TryParse(form["Category"], out Category parsedCategory))
        {
            ticketToUpdate.Category = parsedCategory;
        }

        ticketToUpdate.Priority = Enum.TryParse<Priority>(form["Priority"], out var parsedPriority) ? parsedPriority : ticketToUpdate.Priority;
        
        // Checkbox returns "true" or not present
        ticketToUpdate.IsCompleted = form["IsCompleted"].FirstOrDefault() == "true";

        ticketToUpdate.Assignee = form["Assignee"];

        // You could add journal notes later (not here unless you build a bigger editor)

        await _ticketService.Update(id, ticketToUpdate);

        return RedirectToAction("ViewPast"); // after saving, go back to view tickets
    }
    catch (Exception ex)
    {
        ModelState.AddModelError(string.Empty, $"Error updating ticket: {ex.Message}");
        return View("Edit", ticketToUpdate); // show the form again with existing data
    }
}





    }
}
