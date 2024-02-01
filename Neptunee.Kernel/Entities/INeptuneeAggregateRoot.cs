using Neptunee.DomainEvents;

namespace Neptunee.Entities;

public interface INeptuneeAggregateRoot : INeptuneeEntity
{
    IReadOnlyCollection<INeptuneeDomainEvent> DomainEvents { get; }

    void Clear();
}