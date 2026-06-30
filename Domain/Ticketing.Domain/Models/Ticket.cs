using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Domain.Enums;

namespace Ticketing.Domain.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public TicketStatus TicketStatus { get; set; } = TicketStatus.Created;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; set; }

    }
}
