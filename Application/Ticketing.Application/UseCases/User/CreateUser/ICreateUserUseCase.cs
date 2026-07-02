using System;
using System.Collections.Generic;
using System.Text;

namespace Ticketing.Application.UseCases.User.CreateUser
{
    public interface ICreateUserUseCase
    {
        Task<CreateUserOutput> Execute(CreateUserInput input, CancellationToken cancellationToken);
    }
}
