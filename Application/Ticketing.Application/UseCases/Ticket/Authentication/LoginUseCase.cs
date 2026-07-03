using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Application.Abstractions.Security;
using Microsoft.Extensions.Logging;

namespace Ticketing.Application.UseCases.Ticket.Authentication
{
    public class LoginUseCase(IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        ILogger<LoginUseCase> logger) : ILoginUseCase
    {
        public async Task<LoginOutput> Execute(LoginInput input, CancellationToken cancelationToken)
        {
            var user = await userRepository.GetUserByUserName(input.UserName, cancelationToken);
            if (user == null)
            {
                logger.LogInformation("Login failed for user {UserName}: User not found", input.UserName);
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
                logger.LogInformation("User {UserName} logged in successfully", input.UserName);
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
