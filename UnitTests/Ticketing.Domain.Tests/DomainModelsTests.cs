using Ticketing.Domain.Enums;
using Ticketing.Domain.Models;

namespace Ticketing.Domain.Tests
{
    public  class DomainModelsTests
    {
        [Fact]
        public void Ticket_ShouldSetProperties()
        {
            var ticketTime = DateTime.UtcNow;
            var model = new Ticket {
                Id = 1,
                Title = "Test Ticket",
                Description = "Description",
                UserName = "Name",
                UserEmail = "test@test.com",
                TicketStatus = TicketStatus.Created,
                CreatedAt = ticketTime,
                ClosedAt = ticketTime,
            };

            Assert.Equal(1, model.Id);
            Assert.Equal("Test Ticket", model.Title);
            Assert.Equal("Description", model.Description);
            Assert.Equal("Name", model.UserName);
            Assert.Equal("test@test.com", model.UserEmail);
            Assert.Equal(TicketStatus.Created, model.TicketStatus);
            Assert.Equal(ticketTime, model.CreatedAt);
            Assert.Equal(ticketTime, model.ClosedAt);
        }

        [Fact]
        public void User_ShouldSetProperties()
        {
            var userTime = DateTime.UtcNow;
            var model = new User()
            {
                Id = 1,
                UserName = "testuser",
                UserRoles = UserRoles.Admin,
                IsActive = true,
                PasswordHash = "hashedpassword",
                Email = "test@test.com",
                CreatedAt = userTime

            };
            Assert.Equal(1, model.Id);
            Assert.Equal("testuser", model.UserName);
            Assert.Equal(UserRoles.Admin, model.UserRoles);
            Assert.True(model.IsActive);
            Assert.Equal("hashedpassword", model.PasswordHash);
            Assert.Equal("test@test.com", model.Email);
            Assert.Equal(userTime, model.CreatedAt);

        }

        [Fact]
        public void Enums_ShouldReturnExpectedValues()
        {
            Assert.Equal(0, (int)TicketStatus.Created);
            Assert.Equal(1, (int)TicketStatus.Closed);
            Assert.Equal(2, (int)TicketStatus.Canceled);
            
            Assert.Equal(0, (int)UserRoles.User);
            Assert.Equal(1, (int)UserRoles.Admin);

            Assert.Equal(0, (int)CreateTicketFailureType.None);
            Assert.Equal(1, (int)CreateTicketFailureType.UserNotFound);
            Assert.Equal(2, (int)CreateTicketFailureType.UserInactive);
            Assert.Equal(3, (int)CreateTicketFailureType.DuplicateTicket);
        }

    }
}
