using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Security.Authentication;
using CapstoneTeam11.Models;
using Microsoft.AspNetCore.Identity;

namespace CapstoneTeam11.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IConfiguration configuration)
        {
            var connectionString = configuration["MONGODB_URI"];
            var settings = MongoClientSettings.FromConnectionString(connectionString);

            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };

            var client = new MongoClient(settings);
            var database = client.GetDatabase("TICKLR");
            _usersCollection = database.GetCollection<User>("users");
        }

        public async Task<User?> GetUserById(string id)
        {
            return await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> Create(User user)
        {
            user.Id = ObjectId.GenerateNewId().ToString();
            await _usersCollection.InsertOneAsync(user);
            return user;
        }

        public async Task<ReplaceOneResult> Update(string id, User updatedUser)
        {
            return await _usersCollection.ReplaceOneAsync(user => user.Id == id, updatedUser);
        }


        public async Task<List<User>> GetAllUsers()
        {
            return await _usersCollection.Find(u => true).ToListAsync();
        }

        public async Task Remove(string id)
        {
            await _usersCollection.DeleteOneAsync(u => u.Id == id);
        }
    }
}
