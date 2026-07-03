using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Domain.Enums;

namespace Ticketing.Application.UseCases.Ticket.CreateTicket
{
    public class CreateTicketOutput
    {
        public bool Success { get; set; }
        public int TicketId { get; set; }
        public string? Detail { get; set; }

        public CreateTicketFailureType FailureType { get; init; } = CreateTicketFailureType.None;
    }
}
