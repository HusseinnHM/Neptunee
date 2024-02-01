using System.ComponentModel.DataAnnotations.Schema;
using Neptunee.DomainEvents;
using Neptunee.Entities;

namespace Sample.Domain.Entities;

public class AggregateRoot : Entity, INeptuneeAggregateRoot
{
    public AggregateRoot()
    {
        Id = Guid.NewGuid();
    }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public DateTimeOffset? UtcDateDeleted { get; set; }
    public DateTimeOffset UtcDateCreated { get; set; }
    public DateTimeOffset? UtcDateUpdated { get; set; }

    private readonly List<INeptuneeDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<INeptuneeDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void Clear()
    {
        _domainEvents.Clear();
    }

    protected void AddDomainEvent(INeptuneeDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}