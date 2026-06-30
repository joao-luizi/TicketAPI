using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Application.UseCases.CreateTicket
{
    public class CreateTicketInput
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
}
