using MongoDB.Driver;
using CapstoneTeam11.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;

namespace CapstoneTeam11.Services
{
    public class TicketService : ITicketService
    {
        private readonly IMongoCollection<Ticket> _ticketCollection;

        public TicketService(IConfiguration configuration)
        {
            var connectionString = configuration["MONGODB_URI"];
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("Ticklr");
            _ticketCollection = database.GetCollection<Ticket>("tickets");
        }

        public async Task<List<Ticket>> GetAllTickets()
        {
            return await _ticketCollection.Find(t => true).ToListAsync();
        }

        public async Task<Ticket?> GetTicketById(string id)
        {
            return await _ticketCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Ticket?> Create(Ticket ticket) // <-- Added this Create method
        {
            await _ticketCollection.InsertOneAsync(ticket);
            return ticket;
        }

        public async Task<ReplaceOneResult> Update(string id, Ticket updatedTicket)
        {
            return await _ticketCollection.ReplaceOneAsync(t => t.Id == id, updatedTicket);
        }

        public async Task Remove(string id)
        {
            await _ticketCollection.DeleteOneAsync(t => t.Id == id);
        }

        public int GetTotalTickets()
        {
            return (int)_ticketCollection.AsQueryable().Count();
        }

        public (int open, int closed) GetOpenClosedTicketCounts()
        {
            var open = (int)_ticketCollection.AsQueryable().Count(t => !t.IsCompleted);
var closed = (int)_ticketCollection.AsQueryable().Count(t => t.IsCompleted);
            return (open, closed);
        }

        public Dictionary<string, int> GetTicketsByCategory()
        {
            return _ticketCollection.AsQueryable()
            .GroupBy(t => t.Category.ToString())
            .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}