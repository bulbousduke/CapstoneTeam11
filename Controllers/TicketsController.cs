using Microsoft.AspNetCore.Mvc;

namespace CapstoneTeam11.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Manage()
        {
            return View();
        }

        public IActionResult ViewPast()
        {
            return View();
        }
    }
}