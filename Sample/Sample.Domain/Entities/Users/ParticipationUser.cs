namespace Sample.Domain.Entities;

public class ParticipationUser : Entity, IEntityHasEmail, IEntityHasPassword
{
    private ParticipationUser()
    {
    }

    public ParticipationUser(string name, string email, string passwordHash)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    private readonly List<Ticket> _tickets = new();
    public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();
}