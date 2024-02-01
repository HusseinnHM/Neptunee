using Neptunee.DomainEvents;
using Neptunee.DomainEvents.Dispatcher;
using Neptunee.DomainEvents.Handler;

namespace Neptunee.Dispatchers.DomainEventDispatcher;

public delegate object NeptuneeServiceFactory(Type serviceType);
public sealed class NeptuneeDomainEventDispatcher : INeptuneeDomainEventDispatcher
{
    private readonly NeptuneeServiceFactory _serviceFactory;

    public NeptuneeDomainEventDispatcher(NeptuneeServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }


    public async Task PublishAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
        where TDomainEvent : INeptuneeDomainEvent
    {
        var domainEventHandler = _serviceFactory(typeof(INeptuneeDomainEventHandler<>).MakeGenericType(domainEvent.GetType()));
        await (Task)domainEventHandler
            .GetType()
            .GetMethod(nameof(INeptuneeDomainEventHandler<TDomainEvent>.HandleAsync))!
            .Invoke(domainEventHandler, new object[] { domainEvent, cancellationToken })!;
    }
}