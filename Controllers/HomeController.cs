using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CapstoneTeam11.Services;
using CapstoneTeam11.Models;

namespace CapstoneTeam11.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly TicketService _ticketService;
        private readonly IUserService _userService;

        public HomeController(TicketService ticketService, IUserService userService)
        {
            _ticketService = ticketService;
            _userService = userService;
        }

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
                    var categories = user.AssignedCategories
                        .Select(c => Enum.TryParse<Category>(c, out var cat) ? cat : Category.Other)
                        .T