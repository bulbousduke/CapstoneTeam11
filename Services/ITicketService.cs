using CapstoneTeam11.Models;
using Microsoft.AspNetCore.Authentication;
using MongoDB.Driver;

namespace CapstoneTeam11.Services;

public interface ITicketService 
{
    Task<Ticket?> GetTicketById(string id);
    Task<Ticket?> Create(Ticket ticket);
    Task<List<Ticket>> GetAllTickets();
    Task<ReplaceOneResult> Update(string id, Ticket ticket);
}