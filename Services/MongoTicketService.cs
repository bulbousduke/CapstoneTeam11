using CapstoneTeam11.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CapstoneTeam11.Services
{
    public class MongoTicketService
    {
        
        private readonly IMongoCollection<TicketModel> _ticketCollection;

        public MongoTicketService(IConfiguration configuration)
        {
            var connectionString = configuration["MONGODB_URI"];
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("TICKLR");
            _ticketCollection = database.GetCollection<TicketModel>("tickets");
        }

        public TicketModel? GetTicketById(string id)
        {
            return _ticketCollection.Find(t => t.Id == id).FirstOrDefault();
        }

        public void UpdateTicket(string id, TicketModel updatedTicket)
        {
            _ticketCollection.ReplaceOne(ticket => ticket.Id == id, updatedTicket);
        }

        
        

        public void Create(TicketModel ticket) => _ticketCollection.InsertOne(ticket);

        public List<TicketModel> GetAllTickets() => _ticketCollection.Find(_ => true).ToList();
    }
}