using Microsoft.EntityFrameworkCore;
using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.Core.Abstractions;
using Sample.Application.Tickets.Queries.QueryObjects;
using Sample.Application.Tickets.Queries.Responses;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;
using Sample.SharedKernel.Contracts.Requests;

namespace Sample.Application.Tickets.Queries;

public record GetAllTicketQuery : PagingRequest, INeptuneeRequest<Operation<List<GetAllTicketResponse>>>;

internal class GetAllTicketQueryHandler : INeptuneeRequestHandler<GetAllTicketQuery, Operation<List<GetAllTicketResponse>>>
{
    private readonly IRepository _repository;
    private readonly IHttpResolver _httpResolver;
    private INeptuneeRequestHandler<GetAllTicketQuery, Operation<List<GetAllTicketResponse>>> _neptuneeRequestHandlerImplementation;

    public GetAllTicketQueryHandler(IRepository repository, IHttpResolver httpResolver)
    {
        _repository = repository;
        _httpResolver = httpResolver;
    }

    public async Task<Operation<List<GetAllTicketResponse>>> HandleAsync(GetAllTicketQuery request, CancellationToken cancellationToken = default)
    {
        var currentUserId = _httpResolver.CurrentUserId();
        return await _repository
            .Query<Ticket>()
            .FilterIgnoreSoftDeleted()
            .Where(t => t.ParticipationUserId == currentUserId)
            .Paging(request.PageIndex,request.PageSize)
            .OrderBy(t => t.Event.StartDate <= DateTime.Now)
            .GetMyTicketsResponse(_httpResolver.LanguageKey())
            .ToListAsync(cancellationToken: cancellationToken);
    }
}