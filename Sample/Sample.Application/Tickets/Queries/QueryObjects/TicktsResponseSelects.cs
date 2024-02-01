using Neptunee.EntityFrameworkCore.MultiLanguage;
using Sample.Application.Tickets.Queries.Responses;
using Sample.Domain.Entities;

namespace Sample.Application.Tickets.Queries.QueryObjects;

public static class TicketResponseSelects
{
    public static IQueryable<GetAllTicketResponse> GetMyTicketsResponse(this IQueryable<Ticket> query, string languageKey)
    {
        return query.Select(t => new GetAllTicketResponse
        {
            Id = t.Id,
            EventId = t.EventId,
            EventName = t.Event.Name.GetIn(languageKey)
        });
    }
}