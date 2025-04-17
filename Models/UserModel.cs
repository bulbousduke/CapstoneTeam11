using MongoDB.Bson;

namespace CapstoneTeam11.Models;

public class UserModel
{
    public ObjectId Id { get; set; }

    public AccessLevel AccessLevel { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string Name { get; set; }
}