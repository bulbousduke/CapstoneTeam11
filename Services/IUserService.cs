using CapstoneTeam11.Models;
using MongoDB.Driver;

namespace CapstoneTeam11.Services;

public interface IUserService 
{
    Task<User?> GetUserById(string id);
    Task<User?> Create(User user);
    Task<ReplaceOneResult> Update(string id, User user);
    Task<List<User>> GetAllUsers();
    Task<User?> GetUserByEmail(string email);
    Task Remove(string id);
    bool Register(string name, string email, string password, out string error);
    User? Login(string email, string password);
}