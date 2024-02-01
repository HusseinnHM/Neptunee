using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Domain.Entities;

namespace Sample.Infrastructure.Persistence.EntitiesConfigurations;

public class ParticipationUserConfiguration : IEntityTypeConfiguration<ParticipationUser>
{
    public void Configure(EntityTypeBuilder<ParticipationUser> builder)
    {
        builder.HasIndex(e => e.Email).IsUnique();
    }
}