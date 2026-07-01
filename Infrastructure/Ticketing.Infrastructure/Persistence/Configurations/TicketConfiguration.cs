using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Models;

namespace Ticketing.Infrastructure.Persistence.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Title).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Description).IsRequired().HasMaxLength(500);
            builder.Property(t => t.TicketStatus).IsRequired();
            builder.Property(t => t.CreatedAt).IsRequired();

        }
    }
}

