using Microsoft.Extensions.Logging;
using Moq;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Application.Abstractions.Security;
using Ticketing.Application.UseCases.Authentication;
using Ticketing.Domain.Enums;
using Ticketing.Domain.Models;

namespace Ticketing.Application.Tests.UseCases
{
    public class LoginUseCaseTests
    {
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IPasswordHasher> passwordHasherMock = new();
        private readonly Mock<ITokenService> tokenServiceMock = new();
        private readonly Mock<ILogger<LoginUseCase>> loggerMock = new();

        [Fact]
        public async Task LoginUseCase_ShouldReturnSuccess_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new User
            {
                UserName = "testuser",
                PasswordHash = "hashedpassword",
                Email = "testc@test.com",
                CreatedAt = DateTime.UtcNow,
                UserRoles = UserRoles.User,
            };
            userRepositoryMock.Setup(repo => repo.GetUserByUserName("testuser", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            passwordHasherMock.Setup(hasher => hasher.Verify("password", "hashedpassword")).Returns(true);
            tokenServiceMock.Setup(service => service.GenerateAccessToken(user)).Returns("token");
            var useCase = new LoginUseCase(userRepositoryMock.Object, passwordHasherMock.Object, tokenServiceMock.Object, loggerMock.Object);
            // Act
            var result = await useCase.Execute(new LoginInput { UserName = "testuser", Password = "password" }, CancellationToken.None);
            // Assert
            Assert.True(result.Success);
            Assert.Equal("token", result.Token);
        }
        [Fact]
        public async Task LoginUseCase_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            userRepositoryMock.Setup(repo => repo.GetUserByUserName("nonexistentuser", It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);
            var useCase = new LoginUseCase(userRepositoryMock.Object, passwordHasherMock.Object, tokenServiceMock.Object, loggerMock.Object);
            // Act
            var result = await useCase.Execute(new LoginInput { UserName = "nonexistentuser", Password = "password" }, CancellationToken.None);
            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Token);
            Assert.Equal("User not found", result.Detail);
        }

        [Fact]
        public async Task  LoginUseCase_ShouldReturnFailure_WhenPasswordIsInvalid()
        {
            // Arrange
            var user = new User
            {
                UserName = "testuser",
                PasswordHash = "hashedpassword",
                Email = "admin@admin.com",
            };

            userRepositoryMock
                .Setup(repo => repo.GetUserByUserName("testuser", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            passwordHasherMock
                .Setup(hasher => hasher.Verify("wrongpassword", "hashedpassword"))
                .Returns(false);

            var useCase = new LoginUseCase(
                userRepositoryMock.Object,
                passwordHasherMock.Object,
                tokenServiceMock.Object,
                loggerMock.Object);

            // Act
            var result = await useCase.Execute(new LoginInput { UserName = "testuser", Password = "wrongpassword" }, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Token);
            Assert.Equal("Invalid password", result.Detail);
          

            // Ensure no token was generated when password is invalid
            tokenServiceMock.Verify(s => s.GenerateAccessToken(It.IsAny<User>()), Times.Never);
        }
    }
}
