namespace Neptunee.Entities;

public interface INeptuneeUpdatableEntity : INeptuneeEntity
{
    public DateTimeOffset? UtcDateUpdated { get; set; }
}