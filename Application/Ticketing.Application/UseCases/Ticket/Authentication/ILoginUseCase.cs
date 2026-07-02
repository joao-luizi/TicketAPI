using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Application.UseCases.Ticket.Authentication
{
    public interface ILoginUseCase
    {
        Task<LoginOutput> Execute(LoginInput input, CancellationToken cancelationToken);
    }
}
