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
    public string Id { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("category")]
    [BsonRepresentation(BsonType.String)]
    public Category Category { get; set; }

    [BsonElement("createdDate")]
    public DateTime CreatedDate { get; set; }

    [BsonElement("priority")]
    [BsonRepresentation(BsonType.String)]
    public Priority Priority { get; set; }

    [BsonElement("isCompleted")]
    public bool IsCompleted { get; set; }

    [BsonElement("createdBy")]
    public User CreatedBy { get; set; } = new();

    [BsonElement("assignee")]
    public string? Assignee { get; set; }

    [BsonElement("journalEntries")]
    public List<string> JournalNotes { get; set; } = new();
}
}