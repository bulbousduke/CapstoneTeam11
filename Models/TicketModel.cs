using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CapstoneTeam11.Models
{
    public class TicketModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId TicketId { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserModel? CreatedBy { get; set; }
        public UserModel? Assignee { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }
        public Priority Priority { get; set; }
    }
}