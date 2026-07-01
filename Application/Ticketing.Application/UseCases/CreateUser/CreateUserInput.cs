using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Application.UseCases.CreateUser
{
    public class CreateUserInput
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
