using Neptunee.EntityFrameworkCore.MultiLanguage;
using Sample.Application.Events.Queries.Responses;
using Sample.Domain.Entities;

namespace Sample.Application.Events.Queries.QueryObjects;

public static class EventResponseSelects
{
    public static IQueryable<GetAllEventResponse> GetAllEventResponse(this IQueryable<Event> query, string languageKey)
        => query.Select(e => new GetAllEventResponse()
        {
            Id =e.Id,
            Name = e.Name.GetIn(languageKey),
            Description = e.Description.GetIn(languageKey),
            Location = e.Location.GetIn(languageKey),
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            AvailableTickets = e.AvailableTickets,
            BookedTickets = e.Tickets.Count
        });

    public static IQueryable<GetAllAvailableEventResponse> GetAllAvailableEventResponse(this IQueryable<Event> query, string languageKey, Guid currentUserId)
        => query.Select(e => new GetAllAvailableEventResponse
        {
            Id = e.Id,
            Name = e.Name.GetIn(languageKey),
            Description = e.Description.GetIn(languageKey),
            Location = e.Location.GetIn(languageKey),
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            AvailableTickets = e.AvailableTickets - e.Tickets.Count,
            AlreadyBook = e.Tickets.Any(ep => ep.ParticipationUserId == currentUserId)
        });
}
