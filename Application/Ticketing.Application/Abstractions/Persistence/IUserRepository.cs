using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Domain.Models;

namespace Ticketing.Application.Abstractions.Persistence
{
    public  interface IUserRepository
    {
        Task<User?> GetUserById(int id, CancellationToken cancellationToken);
        Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken);

        Task<User?> GetUserByUserName(string username, CancellationToken cancellationToken);

        Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);

    }
}
