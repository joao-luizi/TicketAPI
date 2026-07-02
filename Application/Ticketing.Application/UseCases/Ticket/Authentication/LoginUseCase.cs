using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Application.Abstractions.Security;

namespace Ticketing.Application.UseCases.Ticket.Authentication
{
    public class LoginUseCase(IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService) : ILoginUseCase
    {
        public async Task<LoginOutput> Execute(LoginInput input, CancellationToken cancelationToken)
        {
            var user = await userRepository.GetUserByUserName(input.UserName, cancelationToken);
            if (user == null)
            {
                return new LoginOutput()
                {
                    Success = false,
                    Token = null,
                    Detail = "User not found",
                };
            }
            if (passwordHasher.Verify(input.Password, user.PasswordHash))
            {
                var token = tokenService.GenerateAccessToken(user);
                return new LoginOutput()
                {
                    Success = true,
                    Token = token,
                    Detail = "Login successful",
                };
            }
            else
            {
                return new LoginOutput()
                {
                    Success = false,
                    Detail = "Invalid password",
                };
            }


        }
    }
}
