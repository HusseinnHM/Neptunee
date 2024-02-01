namespace Sample.Domain.Entities;

public class Ticket : Entity
{
    private Ticket()
    {
    }
    
    public Ticket(Guid eventId, Guid participationUserId)
    {
        EventId = eventId;
        ParticipationUserId = participationUserId;
        DateCreated = DateTime.Now;
    }


    public Guid ParticipationUserId { get; private set; }
    public ParticipationUser ParticipationUser { get; private set; }

    public Guid EventId { get; private set; }
    public Event Event { get; private set; }

    public DateTime DateCreated { get; set; }
}