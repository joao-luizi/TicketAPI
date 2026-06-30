using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Domain.Models;

namespace Ticketing.Infrastructure.Persistence.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        // This is a simple in-memory repository for demonstration purposes.
        private List<Ticket> tickets = new List<Ticket>();

        private int _currentId = 0;
        public async Task<Ticket> CreateTicketAsync(Ticket ticket, CancellationToken cancellationToken)
        {
            //cancellation token is not used because we dont have and addasync method to pass it to,
            //but in a real implementation, you would use it to cancel the operation if needed.
            ticket.Id = ++_currentId;
            tickets.Add(ticket);
            return ticket;
        }

        public async Task<bool> HasDuplicateTicketAsync(int userId, string title, CancellationToken cancellationToken)
        {
            return tickets.Any(t => t.Id == userId && t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }
    }
}
