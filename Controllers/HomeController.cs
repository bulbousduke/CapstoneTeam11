using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CapstoneTeam11.Models;
using MongoDB.Driver;
using CapstoneTeam11.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CapstoneTeam11.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;

        

        public HomeController(ILogger<HomeController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var allUsers = await _userService.GetAllUsers();
            return View(allUsers);
        }

        [Authorize]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        [Authorize]
        public IActionResult EmployeeDashboard()
        {
            return View();
        }

        [Authorize]
        public IActionResult UserDashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}