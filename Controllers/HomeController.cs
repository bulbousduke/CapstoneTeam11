using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CapstoneTeam11.Models;
using MongoDB.Driver;

namespace CapstoneTeam11.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMongoCollection<UserModel> _users;
    
    public HomeController(ILogger<HomeController> logger, IMongoClient mongoClient)
    {
        _logger = logger;

        // define database and collection variables
        var database = mongoClient.GetDatabase("TICKLR");
        _users = database.GetCollection<UserModel>("users");
    }

    public IActionResult Index()
    {
        return View();

        // TEST: displaying data from the database
        var allUsers = _users.Find(_ => true).ToList(); // get all users
        return View(allUsers); // pass data to the view
    }

    public IActionResult Privacy()
    {
        var allUsers = _users.Find(_ => true).ToList(); // get all users
        return View(allUsers); // pass data to the view
        // return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
