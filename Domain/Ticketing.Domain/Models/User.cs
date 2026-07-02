using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Domain.Enums;

namespace Ticketing.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRoles UserRoles { get; set; } = UserRoles.User;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
 
    }
}
