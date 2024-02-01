using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Neptunee.DomainEvents.Dispatcher;
using Neptunee.Entities;

namespace Neptunee.EntityFrameworkCore.Interceptors;

public class PublishNeptuneeDomainEventsInterceptor<TKey> : SaveChangesInterceptor where TKey : struct, IEquatable<TKey>
{
    private readonly INeptuneeDomainEventDispatcher _domainEventDispatcher;

    public PublishNeptuneeDomainEventsInterceptor(INeptuneeDomainEventDispatcher domainEventDispatcher)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    
    
        await DispatchDomainEventsAsync(dbContext, cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }


    private async Task DispatchDomainEventsAsync(DbContext dbContext, CancellationToken cancellationToken)
    {
        var events = new List<Task>();
        foreach (var aggregateRoot in dbContext
                     .ChangeTracker
                     .Entries<INeptuneeAggregateRoot>()
                     .Where(a => a.Entity.DomainEvents.Count != 0)
                     .Select(a => a.Entity))
        {
            events.AddRange(aggregateRoot
                .DomainEvents
                .Select(e => _domainEventDispatcher.PublishAsync(e, cancellationToken)));
            aggregateRoot.Clear();
        }
        await Task.WhenAll(events);
    }
}