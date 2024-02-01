using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Domain.Entities;

namespace Sample.Infrastructure.Persistence.EntitiesConfigurations;

public class EventManagerConfiguration : IEntityTypeConfiguration<EventManager>
{
    public void Configure(EntityTypeBuilder<EventManager> builder)
    {
        builder.HasIndex(e => e.Email).IsUnique();
    }
}