using Microsoft.AspNetCore.Mvc;

namespace CapstoneTeam11.Controllers
{
    public class ViewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}