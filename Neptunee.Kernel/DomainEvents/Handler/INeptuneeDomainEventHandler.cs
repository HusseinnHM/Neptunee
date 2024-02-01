namespace Neptunee.DomainEvents.Handler;

public interface INeptuneeDomainEventHandler<in TDomainEvent> where TDomainEvent : INeptuneeDomainEvent
{
    Task HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
}