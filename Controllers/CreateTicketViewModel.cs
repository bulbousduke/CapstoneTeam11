// CreateTicketViewModel.cs
using CapstoneTeam11.Models;

namespace CapstoneTeam11.ViewModels
{
    public class CreateTicketViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Category Category { get; set; }
        public Priority Priority { get; set; }
    }
}
