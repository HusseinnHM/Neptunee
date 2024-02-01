namespace Sample.Application.Tickets.Queries.Responses;

public class GetAllTicketResponse
{
    public Guid Id { get; init; }
    public Guid EventId { get; init; }
    public string EventName { get; init; }

}