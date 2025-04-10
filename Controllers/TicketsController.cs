using Microsoft.AspNetCore.Mvc;

namespace CapstoneTeam11.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult Create()
        {
            return View("Create/Index");
        }

        public IActionResult Manage()
        {
            return View("Manage/Index");
        }

        public IActionResult ViewPast()
        {
            return View("ViewPast/Index");
        }
    }
}