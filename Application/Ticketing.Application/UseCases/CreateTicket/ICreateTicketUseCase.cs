using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Application.UseCases.CreateTicket
{
    public interface ICreateTicketUseCase
    {
        Task<CreateTicketOutput> CreateTicketAsync(CreateTicketInput input, CancellationToken cancelationToken);
    }
}
