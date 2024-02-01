namespace Neptunee.DomainEvents.Dispatcher;

public interface INeptuneeDomainEventDispatcher
{
    Task PublishAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
        where TDomainEvent : INeptuneeDomainEvent;
    
}