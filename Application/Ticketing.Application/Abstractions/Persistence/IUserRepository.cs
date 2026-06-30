using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Domain.Models;

namespace Ticketing.Application.Abstractions.Persistence
{
    public  interface IUserRepository
    {
        Task<User?> GetUserById(int id, CancellationToken token);
        Task<User?> GetUserByEmail(string email, CancellationToken token);

        Task<User?> GetUserByUserName(string username, CancellationToken token);

    }
}
