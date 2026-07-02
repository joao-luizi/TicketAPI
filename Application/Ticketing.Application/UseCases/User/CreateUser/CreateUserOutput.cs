using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Application.UseCases.User.CreateUser
{
    public  class CreateUserOutput
    {
        public bool Success { get; set; }
        public int UserId { get; set; }
        public string? Detail { get; set; }
    }
}
