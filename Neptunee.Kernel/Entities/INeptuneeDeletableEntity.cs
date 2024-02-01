namespace Neptunee.Entities;

public interface INeptuneeDeletableEntity : INeptuneeEntity
{
    public DateTimeOffset? UtcDateDeleted { get; set; }
}