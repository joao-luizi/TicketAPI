using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Application.UseCases.Authentication
{
    public interface ILoginUseCase
    {
        Task<LoginOutput> Execute(LoginInput input, CancellationToken cancelationToken);
    }
}
