using CapstoneTeam11.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneTeam11.Services
{
    public class TicketService : ITicketService
    {
        private readonly IMongoCollection<Ticket> _ticketCollection;

        public TicketService(IConfiguration configuration)
        {
            var connectionString = configuration["MONGODB_URI"];
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("TICKLR");
            _ticketCollection = database.GetCollection<Ticket>("tickets");
        }

        public async Task<Ticket?> GetTicketById(string id)
        {
            return await _ticketCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ReplaceOneResult> Update(string id, Ticket updatedTicket)
        {
            return await _ticketCollection.ReplaceOneAsync(ticket => ticket.Id == id, updatedTicket);
        }

        public async Task<Ticket?> Create(Ticket ticket)
        {
            ticket.Id = ObjectId.GenerateNewId().ToString();
            await _ticketCollection.InsertOneAsync(ticket);
            return ticket;
        }

        public async Task<List<Ticket>> GetAllTickets()
        {
            return await _ticketCollection.Find(t => true).ToListAsync();
        }
    }
}