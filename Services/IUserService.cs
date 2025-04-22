using CapstoneTeam11.Models;

namespace CapstoneTeam11.Services;

public interface IUserService 
{
    Task<User?> GetUserById(string id);
    Task<User?> Create(User user);
}