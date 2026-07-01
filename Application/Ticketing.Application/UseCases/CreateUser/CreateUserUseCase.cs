
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Application.Abstractions.Security;
using Ticketing.Domain.Models;

namespace Ticketing.Application.UseCases.CreateUser
{
    public  class CreateUserUseCase(IUserRepository userRepository,
        IPasswordHasher passwordHasher) : ICreateUserUseCase
    {
        public async Task<CreateUserOutput> CreateUserAsync(CreateUserInput input, CancellationToken cancellationToken)
        {
            var existingEmailUser = await userRepository.GetUserByEmail(input.Email, cancellationToken);
            var existingUsernameUser = await userRepository.GetUserByUserName(input.UserName, cancellationToken);
            if (existingEmailUser != null)
            {
                return new CreateUserOutput
                {
                    Success = false,
                    Detail = "User with the same email already exists"
                };
            }
            if (existingUsernameUser != null)
            {
                return new CreateUserOutput
                {
                    Success = false,
                    Detail = "User with the same username already exists"
                };
            }

            var newUser = await userRepository.CreateUserAsync(new User
            {
                UserName = input.UserName,
                PasswordHash = passwordHasher.Hash(input.Password),
                Email = input.Email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);

            return new CreateUserOutput
            {
                Success = true,
                UserId = newUser.Id,
                Detail = "User created successfully"
            };
        }
    }
}
