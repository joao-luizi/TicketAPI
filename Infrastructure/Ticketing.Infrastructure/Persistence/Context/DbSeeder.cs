using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Application.Abstractions.Security;
using Ticketing.Domain.Enums;
using Ticketing.Domain.Models;

namespace Ticketing.Infrastructure.Persistence.Context
{
    internal class DbSeeder(TicketingDbContext context,
        IPasswordHasher passwordHasher) : IDbSeeder
    {

        public async Task SeedAsync()
        {
            if (await context.Users.AnyAsync(u => u.UserRoles == UserRoles.Admin))
                return;

            context.Users.Add(new User
            {
                UserName = "admin",
                PasswordHash = passwordHasher.Hash("!#123Admin"),
                UserRoles = UserRoles.Admin
            });

            await context.SaveChangesAsync();
        }

    }
}
