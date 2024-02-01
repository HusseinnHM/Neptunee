namespace Neptunee.Entities;

public interface INeptuneeEntity<TKey> : INeptuneeEntity where TKey : struct, IEquatable<TKey>
{
    public TKey Id { get; set; }
}

