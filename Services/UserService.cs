using CapstoneTeam11.Models;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace CapstoneTeam11.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("users");
        }

        public async Task<User?> GetUserById(string id) =>
            await _users.Find(u => u.Id == id).FirstOrDefaultAsync();

        public async Task<User?> Create(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<ReplaceOneResult> Update(string id, User user) =>
            await _users.ReplaceOneAsync(u => u.Id == id, user);

        public async Task<List<User>> GetAllUsers() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task Remove(string id) =>
            await _users.DeleteOneAsync(u => u.Id == id);

    public bool Register(string name, string email, string password, out string error)
    {
        if (_users.Find(u => u.Email == email).Any())
        {
            error = "Email already registered.";
            return false;
        }

        var newUser = new User
        {
            Name = name,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            AccessLevel = AccessLevel.User
        };

        _users.InsertOne(newUser);
        error = string.Empty;
        return true;
    }

        public User? Login(string email, string password)
{
    var user = _users.Find(u => u.Email == email).FirstOrDefault();
    return user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)
        ? user
        : null;
}

public async Task<bool> UpdateAccessLevel(string userId, AccessLevel newRole)
{
    var user = await GetUserById(userId);
    if (user == null) return false;

    user.AccessLevel = newRole;
    await Update(userId, user);
    return true;
}

public async Task<User?> GetUserByEmail(string email)
{
    return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
}

public int GetTotalUsers()
{
    return (int)_users.AsQueryable().Count();
}

public Dictionary<string, int> GetUsersByAccessLevel()
{
    return _users.AsQueryable()
        .GroupBy(u => u.AccessLevel)
        .ToDictionary(g => g.Key.ToString(), g => g.Count());
}
    
    }
}