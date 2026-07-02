using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Application.UseCases.Ticket.Authentication
{
    public class LoginInput
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
