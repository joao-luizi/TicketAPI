using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; }

        public int FailedLoginAttempts { get; set; }
        public DateTime? LockoutEnd { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
    }
}
