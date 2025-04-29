using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CapstoneTeam11.Models;
 public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")] // Map MongoDB _id field
        public string? UserId { get; set; }

        [BsonElement("accessLevel")]
        [BsonRepresentation(BsonType.String)] // Store enum as a string
        public AccessLevel AccessLevel { get; set; }

        [BsonElement("email")]
        public required string Email { get; set; }

        [BsonElement("password")]
        public required string Password { get; set; }

        [BsonElement("name")]
        public required string Name { get; set; }
    }