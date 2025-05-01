using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Security.Authentication;
using CapstoneTeam11.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.UserSecrets;

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

        public bool Register(string name, string email, string password, out string error)
        {
            error = "";
            if(_usersCollection.Find(u => u.Email == email).Any())
            {
                error = "There is already account registered under that email.";
                return false;
            }

            var user = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = name,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                AccessLevel = AccessLevel.User
            };

            _usersCollection.InsertOneAsync(user);
            return true;
        }

        public User? Login(string email, string password)
        {
            var user = _usersCollection.Find(u => u.Email == email).FirstOrDefault();
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }

            return null;
        }
    }
}
