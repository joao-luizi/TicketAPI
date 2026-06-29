using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Domain.Models
{
    public class Ticket
    {
        int Id { get; set; }
        string Title { get; set; } = string.Empty;  
        string Description { get; set; } = string.Empty;
        string UserName { get; set; } = string.Empty;
        string UserEmail { get; set; } = string.Empty;
        DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        DateTime? ClosedAt { get; set; }

    }
}
