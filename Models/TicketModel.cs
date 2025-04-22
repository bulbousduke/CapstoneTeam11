using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CapstoneTeam11.Models
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public User? CreatedBy { get; set; }
        public User? Assignee { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public Priority Priority { get; set; }
    }
}