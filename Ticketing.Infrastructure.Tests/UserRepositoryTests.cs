using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Enums;
using Ticketing.Domain.Models;
using Ticketing.Infrastructure.Persistence.Context;
using Ticketing.Infrastructure.Persistence.Repositories;

namespace Ticketing.Infrastructure.Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task CreateUserAsync_ShouldAddUserToDatabase()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<TicketingDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            using var context = new TicketingDbContext(options);
            var userrepo = new UserRepository(context);
            var currentTime = DateTime.UtcNow;
            var user = new User
            {
                UserName = "TestUser",
                Email = "test@email.com",
                PasswordHash = "hashedpassword",
                CreatedAt = currentTime,
                IsActive = true,
                UserRoles = UserRoles.User
            };

            var createdUser = await userrepo.CreateUserAsync(user, CancellationToken.None);

            Assert.NotNull(createdUser);
            Assert.Equal(user.UserName, createdUser.UserName);
            Assert.Equal(user.Email, createdUser.Email);
            Assert.Equal(user.PasswordHash, createdUser.PasswordHash);
            Assert.Equal(user.CreatedAt, createdUser.CreatedAt);
            Assert.Equal(user.IsActive, createdUser.IsActive);
            Assert.Equal(user.UserRoles, createdUser.UserRoles);
            using var cleanup = new TicketingDbContext(options);
            cleanup.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnUser_WhenUserExists()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<TicketingDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            // Arrange - write with one context and dispose it
            var creationTime = DateTime.UtcNow;
            var insertedId = 0;
            using (var writeContext = new TicketingDbContext(options))
            {
                var writeRepo = new UserRepository(writeContext);
                var user = new User 
                { 
                    UserName = "TestUser", 
                    Email = "test@teste.com", 
                    PasswordHash = "hashed", 
                    CreatedAt = creationTime, 
                    IsActive = true, 
                    UserRoles = UserRoles.User };
                await writeRepo.CreateUserAsync(user, CancellationToken.None);
                insertedId = user.Id;
            }

            // Act - read with a new context
            using (var readContext = new TicketingDbContext(options))
            {
                var readRepo = new UserRepository(readContext);
                var retrieved = await readRepo.GetUserByEmail("test@teste.com", CancellationToken.None);

                // Assert
                Assert.NotNull(retrieved);
                Assert.Equal(insertedId, retrieved!.Id);
                Assert.Equal("test@teste.com", retrieved!.Email);
                Assert.Equal("TestUser", retrieved.UserName);
                Assert.Equal("hashed", retrieved.PasswordHash);
                Assert.Equal(UserRoles.User, retrieved.UserRoles);
                Assert.Equal(creationTime, retrieved.CreatedAt);
                Assert.True(retrieved!.IsActive);
            }

            // Cleanup (optional)
            using var cleanup = new TicketingDbContext(options);
            cleanup.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<TicketingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new TicketingDbContext(options);
            var repo = new UserRepository(context);

            var result = await repo.GetUserByEmail("nonexistent@domain.test", CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByUserName_ShouldReturnUser_WhenUserExists()
        {
            var dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<TicketingDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            // Arrange - write with one context and dispose it
            User user;
            using (var writeContext = new TicketingDbContext(options))
            {
                var writeRepo = new UserRepository(writeContext);
                user = new User
                {
                    UserName = "TestUser",
                    Email = "test@teste.com",
                    PasswordHash = "hashed",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    UserRoles = UserRoles.User
                };
                await writeRepo.CreateUserAsync(user, CancellationToken.None);
              
            }

            // Act - read with a new context
            using (var readContext = new TicketingDbContext(options))
            {
                var readRepo = new UserRepository(readContext);
                var retrieved = await readRepo.GetUserByUserName("TestUser", CancellationToken.None);

                // Assert
                Assert.NotNull(retrieved);
                Assert.Equal(user.Id, retrieved!.Id);
                Assert.Equal(user.Email, retrieved!.Email);
                Assert.Equal(user.UserName, retrieved.UserName);
                Assert.Equal(user.PasswordHash, retrieved.PasswordHash);
                Assert.Equal(UserRoles.User, retrieved.UserRoles);
                Assert.Equal(user.CreatedAt, retrieved.CreatedAt);
                Assert.True(retrieved!.IsActive);
            }

            // Cleanup (optional)
            using var cleanup = new TicketingDbContext(options);
            cleanup.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetUserByUsername_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<TicketingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new TicketingDbContext(options);
            var repo = new UserRepository(context);

            var result = await repo.GetUserByUserName("nonexistent", CancellationToken.None);

            Assert.Null(result);
        }
    }
}
