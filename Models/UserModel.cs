using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CapstoneTeam11.Models;
 public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } // MongoDB _id field 

    [BsonElement("UserId")]
    public string UserId { get; set; }

    [BsonElement("AccessLevel")]
    [BsonRepresentation(BsonType.String)] 
    public AccessLevel AccessLevel { get; set; }

    [BsonElement("Email")]
    public string Email { get; set; }

    [BsonElement("Name")]
    public string Name { get; set; }

    [BsonElement("PasswordHash")]
    public string PasswordHash { get; set; }

    [BsonElement("AssignedCategories")]
    public List<string> AssignedCategories { get; set; } = new();

    
}