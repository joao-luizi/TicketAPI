using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Application.Abstractions.Security;
using Ticketing.Application.UseCases.User.CreateUser;
using Ticketing.Domain.Enums;
using Ticketing.Domain.Models;

namespace Ticketing.Application.Tests.UseCases
{
    public class CreateUserUseCaseTests
    {
        [Fact]
        public async Task Execute_ValidInput_CreatesUserAndReturnsId()
        {
            var userRepo = new Mock<IUserRepository>();
            var logger = new Mock<ILogger<CreateUserUseCase>>();
            var passWordHasher = new Mock<IPasswordHasher>();
            userRepo.Setup(x => x.GetUserByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((User?)null);
            userRepo.Setup(x => x.GetUserByUserName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((User?)null);
            userRepo.Setup(x => x.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((User user, CancellationToken ct) => user);
            passWordHasher.Setup(x => x.Hash(It.IsAny<string>()))
                          .Returns("hashedpassword");
            var useCase = new CreateUserUseCase(userRepo.Object, passWordHasher.Object, logger.Object);
            var result = await useCase.Execute(new CreateUserInput()
            {
                Email = "unique@unique.com",
                UserName = "uniquename",
                Password = "password123"

            }, CancellationToken.None);
            Assert.True(result.Success);
            Assert.Equal(0, result.UserId); 
            userRepo.Verify(x => x.CreateUserAsync(It.Is<User>(u => u.Email == "unique@unique.com" && u.UserName == "uniquename" && u.PasswordHash == "hashedpassword"), It.IsAny<CancellationToken>()), Times.Once);
        }

         [Fact]
        public async Task Execute_UserEmailAlreadyExists_ReturnsUserAlreadyExists()
        {
            var userRepo = new Mock<IUserRepository>();
            var logger = new Mock<ILogger<CreateUserUseCase>>();
            var passWordHasher = new Mock<IPasswordHasher>();
            userRepo.Setup(x => x.GetUserByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new User()
                    {
                        UserName = "username",
                        Email = "admin@admin.com",
                        PasswordHash = "hashedpassword",
                        UserRoles = UserRoles.Admin,
                        CreatedAt = DateTime.UtcNow,
                        Id = 99,
                        IsActive = true
                    });
            var useCase = new CreateUserUseCase(userRepo.Object, passWordHasher.Object, logger.Object);
            var result = await useCase.Execute(new CreateUserInput()
            {
                
                Email = "admin@admin.com",
                UserName = "uniquename",

            }, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("User with the same email already exists", result.Detail);
        }

        [Fact]
        public async Task Execute_UserNameAlreadyExists_ReturnsUserAlreadyExists()
        {
            var userRepo = new Mock<IUserRepository>();
            var logger = new Mock<ILogger<CreateUserUseCase>>();
            var passWordHasher = new Mock<IPasswordHasher>();
            userRepo.Setup(x => x.GetUserByUserName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new User()
                    {
                        UserName = "username",
                        Email = "admin@admin.com",
                        PasswordHash = "hashedpassword",
                        UserRoles = UserRoles.Admin,
                        CreatedAt = DateTime.UtcNow,
                        Id = 99,
                        IsActive = true
                    });
            var useCase = new CreateUserUseCase(userRepo.Object, passWordHasher.Object, logger.Object);
            var result = await useCase.Execute(new CreateUserInput()
            {

                Email = "unique@unique.com",
                UserName = "username",

            }, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("User with the same username already exists", result.Detail);
        }
    }
}
