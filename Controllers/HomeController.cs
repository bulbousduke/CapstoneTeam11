using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CapstoneTeam11.Models;
using MongoDB.Driver;
using CapstoneTeam11.Services;
using System.Threading.Tasks;

namespace CapstoneTeam11.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMongoCollection<User> _users;
        private readonly UserService _userService;
        
        public HomeController(ILogger<HomeController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;

            // define database and collection variables
        }

        public IActionResult Index()
        {
            return View();

            // TEST: displaying data from the database
            // var allUsers = _users.Find(_ => true).ToList(); // get all users
            // return View(allUsers); // pass data to the view
        }

        public async Task<IActionResult> Privacy()
        {
            var allUsers = await _userService.GetAllUsers();
            return View(allUsers); // pass data to the view
            // return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Correct placement of Help method inside HomeController class
        public IActionResult Help()
        {
            return View(); // returns the Help view
        }
    }
}
