using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Neptunee.Clock;
using Neptunee.Entities;

namespace Neptunee.EntityFrameworkCore.Interceptors;

public class UpdateAuditableNeptuneeEntitiesInterceptor<TKey> : SaveChangesInterceptor where TKey : struct, IEquatable<TKey>
{
    private readonly INeptuneeClock _clock;

    public UpdateAuditableNeptuneeEntitiesInterceptor(INeptuneeClock clock)
    {
        _clock = clock;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        UpdateAuditableEntities(dbContext, _clock.UtcNow);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext dbContext, DateTimeOffset dateTimeNow)
    {
        foreach (var entityEntry in dbContext.ChangeTracker.Entries<INeptuneeEntity>())
        {
            switch (entityEntry)
            {
                case { State: EntityState.Added, Entity: INeptuneeCreatableEntity }:
                    entityEntry.Property(nameof(INeptuneeCreatableEntity.UtcDateCreated)).CurrentValue = dateTimeNow;
                    break;
                case { State: EntityState.Modified, Entity: INeptuneeDeletableEntity }
                    when entityEntry.Property(nameof(INeptuneeDeletableEntity.UtcDateDeleted)).CurrentValue is not null:
                    entityEntry.Property(nameof(INeptuneeDeletableEntity.UtcDateDeleted)).CurrentValue = dateTimeNow;
                    break;
                case { State: EntityState.Modified, Entity: INeptuneeUpdatableEntity }:
                    entityEntry.Property(nameof(INeptuneeUpdatableEntity.UtcDateUpdated)).CurrentValue = dateTimeNow;
                    break;
            }
        }
    }
}