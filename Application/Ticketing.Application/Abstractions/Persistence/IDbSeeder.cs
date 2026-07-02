using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Application.Abstractions.Security;

namespace Ticketing.Application.Abstractions.Persistence
{
    public  interface IDbSeeder
    {
        Task SeedAsync();
    }
}
