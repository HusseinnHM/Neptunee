namespace Sample.Application.Events.Queries.Responses;

public record GetAllAvailableEventResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string Location { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int AvailableTickets { get; init; }
    public bool AlreadyBook { get; init; }
}