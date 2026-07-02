using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Application.UseCases.Ticket.Authentication
{
    public  class LoginOutput
    {
        public bool Success { get; set; }
        public string? Token { get; set; } = null;

        public string? Detail { get; set; } = null;
    }
}
