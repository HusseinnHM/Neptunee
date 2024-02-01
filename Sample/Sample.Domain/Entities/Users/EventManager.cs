namespace Sample.Domain.Entities;

public class EventManager : Entity,IEntityHasEmail, IEntityHasPassword
{
    private EventManager()
    {
    }
    public EventManager(string name, string email,string passwordHash)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
    }
    public string Name { get; private set; }
    public string PasswordHash { get; private set; }
    public string Email { get; private set; }

    private readonly List<Event> _createdEvents = new();
    public IReadOnlyCollection<Event> CreatedEvents => _createdEvents.AsReadOnly();
}