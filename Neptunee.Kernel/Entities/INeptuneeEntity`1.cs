namespace Neptunee.Entities;

public interface INeptuneeEntity<TKey> : INeptuneeEntity
{
    public TKey Id { get; set; }
}