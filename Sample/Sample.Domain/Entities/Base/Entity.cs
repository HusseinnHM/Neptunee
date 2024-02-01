using System.ComponentModel.DataAnnotations.Schema;
using Neptunee.Entities;

namespace Sample.Domain.Entities;


public class Entity :  INeptuneeAuditableEntity<Guid>
{
    public Entity()
    {
        Id = Guid.NewGuid();
    }
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get;  set; }
    public DateTimeOffset? UtcDateDeleted { get; set; }
    public DateTimeOffset UtcDateCreated { get; set; }
    public DateTimeOffset? UtcDateUpdated { get; set; }
}

public interface IEntityHasEmail
{
    public string Email { get; }
}public interface IEntityHasPassword
{
    public string PasswordHash { get; }
}