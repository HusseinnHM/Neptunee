using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Neptunee.EntityFrameworkCore.MultiLanguage.Extensions;
using Sample.Application.Core.Abstractions.Data;
using Sample.Domain.Entities;

namespace Sample.Infrastructure.Persistence.Context;

public sealed class SampleDbContext : DbContext, ISampleDbContext
{
    public SampleDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<EventManager> EventManagers => Set<EventManager>();
    public DbSet<ParticipationUser> ParticipationUsers => Set<ParticipationUser>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureMultiLanguage(Database);
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), t => t.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));
    }
}