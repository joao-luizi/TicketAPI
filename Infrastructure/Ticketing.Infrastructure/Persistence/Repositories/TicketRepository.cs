using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Domain.Models;
using Ticketing.Infrastructure.Persistence.Context;

namespace Ticketing.Infrastructure.Persistence.Repositories
{
    public class TicketRepository(TicketingDbContext context) : ITicketRepository
    {
        public async Task<Ticket> CreateTicketAsync(Ticket ticket, CancellationToken cancellationToken)
        {

            await context.Tickets.AddAsync(ticket, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return ticket;
            
        }

        public async Task<bool> HasDuplicateTicketAsync(string userName, string title, CancellationToken cancellationToken)
        {
            return await context.Tickets.AnyAsync(t => t.UserName == userName && t.Title == title, cancellationToken: cancellationToken);
      
        }
    }
}
