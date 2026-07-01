using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Domain.Models;

namespace Ticketing.Application.Abstractions.Persistence
{
    public interface ITicketRepository
    {
         Task<Ticket> CreateTicketAsync(Ticket ticket, CancellationToken cancellationToken);


        Task<bool> HasDuplicateTicketAsync(string username, string title, CancellationToken cancellationToken);
       
    }
}
