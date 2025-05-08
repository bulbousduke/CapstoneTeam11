using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CapstoneTeam11.Models;
 public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("UserId")]
    public string UserId { get; set; } = string.Empty;

    [BsonElement("AccessLevel")]
    [BsonRepresentation(BsonType.String)] 
    public AccessLevel AccessLevel { get; set; }

    [BsonElement("Email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("PasswordHash")]
    public string PasswordHash { get; set; } = string.Empty;

    [BsonElement("AssignedCategories")]
    public List<string> AssignedCategories { get; set; } = new();
}