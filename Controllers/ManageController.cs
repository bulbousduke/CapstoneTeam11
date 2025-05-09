using Microsoft.AspNetCore.Mvc;

namespace CapstoneTeam11.Controllers
{
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}