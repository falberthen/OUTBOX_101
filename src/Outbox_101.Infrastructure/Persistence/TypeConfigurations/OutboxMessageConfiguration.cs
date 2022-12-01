using Microsoft.EntityFrameworkCore;
using Outbox_101.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Outbox_101.Infrastructure.Persistence.TypeConfigurations;

internal class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.Payload);

        builder.Property(e => e.Type);

        builder.Property(e => e.OcurredAt);

        builder.Property(e => e.ProcessedAt);
    }
}