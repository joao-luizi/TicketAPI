using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Domain.Models;

namespace Ticketing.Infrastructure.Persistence.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private List<User> users = new List<User>();
        int currentId = 0;

        public async Task<User?> GetUserByEmail(string email, CancellationToken token)
        {
            return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<User?> GetUserById(int id, CancellationToken token)
        {
            return users.FirstOrDefault(u => u.Id == id);

        }

        public async Task<User?> GetUserByUserName(string username, CancellationToken token)
        {
            return users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
