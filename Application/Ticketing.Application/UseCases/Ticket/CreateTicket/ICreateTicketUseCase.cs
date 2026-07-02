using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Application.UseCases.Ticket.CreateTicket
{
    public interface ICreateTicketUseCase
    {
        Task<CreateTicketOutput> Execute(CreateTicketInput input, CancellationToken cancelationToken);
    }
}
