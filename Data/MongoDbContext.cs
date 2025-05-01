using MongoDB.Driver;
using CapstoneTeam11.Models;

namespace CapstoneTeam11.Data;
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext()
    {
        var client = new MongoClient("mongodb://localhost:27017"); // Replace with your connection string
        _database = client.GetDatabase("YourAppDb");
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
}