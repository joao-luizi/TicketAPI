using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Domain.Models;
using Ticketing.Infrastructure.Persistence.Context;

namespace Ticketing.Infrastructure.Persistence.Repositories
{
    internal class UserRepository (TicketingDbContext context): IUserRepository
    {

        public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
        {

            await context.Users.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return user;

        }
        public async Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
           return await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }

        public async Task<User?> GetUserById(int id, CancellationToken cancellationToken)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        }

        public async Task<User?> GetUserByUserName(string username, CancellationToken cancellationToken)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }
    }
}
