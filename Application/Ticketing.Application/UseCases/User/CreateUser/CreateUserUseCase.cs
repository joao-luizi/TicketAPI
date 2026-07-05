using Microsoft.Extensions.Logging;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Application.Abstractions.Security;

namespace Ticketing.Application.UseCases.User.CreateUser
{
    public  class CreateUserUseCase(IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ILogger<CreateUserUseCase> logger) : ICreateUserUseCase
    {
        public async Task<CreateUserOutput> Execute(CreateUserInput input, CancellationToken cancellationToken)
        {
            var existingEmailUser = await userRepository.GetUserByEmail(input.Email, cancellationToken);
            var existingUsernameUser = await userRepository.GetUserByUserName(input.UserName, cancellationToken);
            if (existingEmailUser != null)
            {
                logger.LogWarning("Attempt to create user with existing email: {Email}", input.Email);
                return new CreateUserOutput
                {
                    Success = false,
                    Detail = "User with the same email already exists"
                };
            }
            if (existingUsernameUser != null)
            {
                logger.LogWarning("Attempt to create user with existing username: {UserName}", input.UserName);
                return new CreateUserOutput
                {
                    Success = false,
                    Detail = "User with the same username already exists"
                };
            }

            var newUser = await userRepository.CreateUserAsync(new Domain.Models.User
            {
                UserName = input.UserName,
                PasswordHash = passwordHasher.Hash(input.Password),
                Email = input.Email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);

            logger.LogInformation("User created successfully with ID: {UserId}", newUser.Id);

            return new CreateUserOutput
            {
                Success = true,
                UserId = newUser.Id,
                Detail = "User created successfully"
            };
        }
    }
}
