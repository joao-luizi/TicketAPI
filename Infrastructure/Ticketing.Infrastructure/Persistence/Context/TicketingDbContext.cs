using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Models;


namespace Ticketing.Infrastructure.Persistence.Context
{
    public sealed class TicketingDbContext(DbContextOptions<TicketingDbContext> options) : DbContext(options)
    {
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketingDbContext).Assembly);
        }
    }

}