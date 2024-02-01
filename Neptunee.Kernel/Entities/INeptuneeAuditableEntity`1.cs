namespace Neptunee.Entities;

public interface INeptuneeAuditableEntity<TKey> : INeptuneeEntity<TKey>,INeptuneeAuditableEntity where TKey : struct, IEquatable<TKey>
{
}