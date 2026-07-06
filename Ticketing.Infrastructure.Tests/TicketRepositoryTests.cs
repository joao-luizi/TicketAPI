using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Ticketing.Domain.Models;
using Ticketing.Infrastructure.Persistence.Context;
using Ticketing.Infrastructure.Persistence.Repositories;

namespace Ticketing.Infrastructure.Tests
{
    public  class TicketRepositoryTests
    {
        [Fact]
        public async Task CreateTicketAsync_ShouldAddTicketToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TicketingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using var context = new TicketingDbContext(options);
            var repository = new TicketRepository(context);
            var ticket = new Ticket
            {
                UserName = "testuser",
                Title = "Test Ticket",
                Description = "This is a test ticket."
            };
            // Act
            var createdTicket = await repository.CreateTicketAsync(ticket, CancellationToken.None);
            // Assert
            Assert.NotNull(createdTicket);
            Assert.Equal(ticket.UserName, createdTicket.UserName);
            Assert.Equal(ticket.Title, createdTicket.Title);
            Assert.Equal(ticket.Description, createdTicket.Description);
        }

        [Fact]
        public async Task HasDuplicateTicketAsync_ShouldReturnTrue_WhenDuplicateExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TicketingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using var context = new TicketingDbContext(options);
            var repository = new TicketRepository(context);
            var ticket = new Ticket
            {
                UserName = "testuser",
                Title = "Test Ticket",
                Description = "This is a test ticket."
            };
            await repository.CreateTicketAsync(ticket, CancellationToken.None);
            // Act
            var hasDuplicate = await repository.HasDuplicateTicketAsync("testuser", "Test Ticket", CancellationToken.None);
            // Assert
            Assert.True(hasDuplicate);
        }

        [Fact]
        public async Task HasDuplicateTicket_ShouldReturnFalse_WhenNoDuplicateExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TicketingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using var context = new TicketingDbContext(options);
            var repository = new TicketRepository(context);
            // Act
            var hasDuplicate = await repository.HasDuplicateTicketAsync("testuser", "Nonexistent Ticket", CancellationToken.None);
            // Assert
            Assert.False(hasDuplicate);
        }
    }
}
