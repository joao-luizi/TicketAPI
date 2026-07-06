using Microsoft.Extensions.Logging;
using Moq;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Application.UseCases.Ticket.CreateTicket;
using Ticketing.Domain.Enums;
using Ticketing.Domain.Models;

namespace Ticketing.Application.Tests.UseCases
{
    public class CreateTicketUseCaseTests
    {
        [Fact]
        public async Task Execute_UserNotFound_ReturnsUserNotFound()
        {
            var ticketRepo = new Mock<ITicketRepository>();
            var userRepo = new Mock<IUserRepository>();
            var logger = new Mock<ILogger<CreateTicketUseCase>>();

            userRepo.Setup(x => x.GetUserByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((User?)null);

            var useCase = new CreateTicketUseCase(ticketRepo.Object, userRepo.Object, logger.Object);

            var input = new CreateTicketInput
            {
                Title = "T",
                Description = "D",
                UserEmail = "noone@nowhere"
            };

            var result = await useCase.Execute(input, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal(CreateTicketFailureType.UserNotFound, result.FailureType);
            Assert.Equal("User not found", result.Detail);
        }

        [Fact]
        public async Task Execute_UserInactive_ReturnsUserInactive()
        {
            var ticketRepo = new Mock<ITicketRepository>();
            var userRepo = new Mock<IUserRepository>();
            var logger = new Mock<ILogger<CreateTicketUseCase>>();

            userRepo.Setup(x => x.GetUserByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new User { UserName = "u", Email = "u@u.com", IsActive = false });

            var useCase = new CreateTicketUseCase(ticketRepo.Object, userRepo.Object, logger.Object);

            var input = new CreateTicketInput
            {
                Title = "T",
                Description = "D",
                UserEmail = "u@u.com"
            };

            var result = await useCase.Execute(input, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal(CreateTicketFailureType.UserInactive, result.FailureType);
            Assert.Equal("User is not active", result.Detail);
        }

        [Fact]
        public async Task Execute_DuplicateTitle_ReturnsDuplicateTicket()
        {
            var ticketRepo = new Mock<ITicketRepository>();
            var userRepo = new Mock<IUserRepository>();
            var logger = new Mock<ILogger<CreateTicketUseCase>>();

            var user = new User { UserName = "tester", Email = "tester@t.com", IsActive = true };

            userRepo.Setup(x => x.GetUserByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

            ticketRepo.Setup(x => x.HasDuplicateTicketAsync(user.UserName, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true);

            var useCase = new CreateTicketUseCase(ticketRepo.Object, userRepo.Object, logger.Object);

            var input = new CreateTicketInput
            {
                Title = "Duplicate",
                Description = "D",
                UserEmail = user.Email
            };

            var result = await useCase.Execute(input, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal(CreateTicketFailureType.DuplicateTicket, result.FailureType);
            Assert.Equal("Duplicate ticket title for the user", result.Detail);
        }

        [Fact]
        public async Task Execute_ValidInput_CreatesTicketAndReturnsId()
        {
            var ticketRepo = new Mock<ITicketRepository>();
            var userRepo = new Mock<IUserRepository>();
            var logger = new Mock<ILogger<CreateTicketUseCase>>();

            var user = new User { UserName = "tester", Email = "tester@t.com", IsActive = true };

            userRepo.Setup(x => x.GetUserByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(user);

            ticketRepo.Setup(x => x.HasDuplicateTicketAsync(user.UserName, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(false);

            ticketRepo.Setup(x => x.CreateTicketAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync((Ticket t, CancellationToken _) =>
                      {
                         
                          t.Id = 99;
                          return t;
                      });

            var useCase = new CreateTicketUseCase(ticketRepo.Object, userRepo.Object, logger.Object);

            var input = new CreateTicketInput
            {
                Title = "New ticket",
                Description = "Description",
                UserEmail = user.Email
            };

            var result = await useCase.Execute(input, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(99, result.TicketId);
            Assert.Equal("Ticket created successfully", result.Detail);

            ticketRepo.Verify(x => x.CreateTicketAsync(It.Is<Ticket>(t =>
                t.Title == input.Title &&
                t.Description == input.Description &&
                t.UserName == user.UserName &&
                t.UserEmail == user.Email
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
