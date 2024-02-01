namespace Neptunee.Entities;

public interface INeptuneeCreatableEntity : INeptuneeEntity
{
    public DateTimeOffset UtcDateCreated { get; set; }
}