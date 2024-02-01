
using Neptunee.EntityFrameworkCore.MultiLanguage.Types;
using Neptunee.OperationResponse;

namespace Sample.Domain.Entities;

public class Event : Entity
{
    private Event()
    {
    }

    public Event(string name, string description, string location, DateTime startDate, DateTime endDate, int availableTickets, Guid creatorId,string defaultLanguageKey)
    {
        Name = new(defaultLanguageKey,name);
        Description = new(defaultLanguageKey,description);
        Location = new(defaultLanguageKey,location);
        StartDate = startDate;
        EndDate = endDate;
        AvailableTickets = availableTickets;
        EventManagerId = creatorId;
        ConcurrencyStamp = Guid.NewGuid();
    }

    public MultiLanguageProperty Name { get; private set; }
    public MultiLanguageProperty Description { get; private set; }
    public MultiLanguageProperty Location { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int AvailableTickets { get; private set; }

    public Guid EventManagerId { get; private set; }
    public EventManager EventManager { get; set; }

    public Guid ConcurrencyStamp { get; private set; }

    private readonly List<Ticket> _tickets = new();
    public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();

    public Result Modify(string name, string description, string location, DateTime startDate, DateTime endDate, int availableTickets, string languageKey)
    {
        if (_tickets.Count > availableTickets)
        {
            return Result.BadRequest(Errors.Events.AvailableTickets(_tickets.Count));
        }

        if (AvailableTickets != availableTickets)
        {
            ConcurrencyStamp = Guid.NewGuid();
        }

        Name.Upsert(languageKey, name);
        Description.Upsert(languageKey, description);
        Location.Upsert(languageKey, location);
        StartDate = startDate;
        EndDate = endDate;
        AvailableTickets = availableTickets;
        return Result.Ok();
    }

    public Result Book(Guid participationUserId)
    {
        if (_tickets.Any(t => t.ParticipationUserId == participationUserId))
        {
            return Result.BadRequest(Errors.Tickets.AlreadyBooked);
        }

        if (_tickets.Count == AvailableTickets)
        {
            return Result.BadRequest(Errors.Tickets.FullBooking,"Sorry");
        }

        ConcurrencyStamp = Guid.NewGuid();
        _tickets.Add(new Ticket(Id, participationUserId));
        return Result.Ok();
    }
}