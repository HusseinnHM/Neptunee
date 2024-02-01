using Microsoft.EntityFrameworkCore;
using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.Core.Abstractions;
using Sample.Application.Events.Queries.QueryObjects;
using Sample.Application.Events.Queries.Responses;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;

namespace Sample.Application.Events.Queries.Handlers;

public class EventQueryHandler :
    INeptuneeRequestHandler<GetAllEventQuery, Operation<List<GetAllEventResponse>>>,
    INeptuneeRequestHandler<GetAllAvailableEventQuery, Operation<List<GetAllAvailableEventResponse>>>
{
    private readonly IHttpResolver _httpResolver;
    private readonly IRepository _repository;

    public EventQueryHandler(IRepository repository, IHttpResolver httpResolver)
    {
        _repository = repository;
        _httpResolver = httpResolver;
    }

    public async Task<Operation<List<GetAllEventResponse>>> HandleAsync(GetAllEventQuery request, CancellationToken cancellationToken)
        => await _repository
            .Query<Event>()
            .FilterIgnoreSoftDeleted()
            .Where(e => e.EventManagerId == _httpResolver.CurrentUserId())
            .OrderBy(e => e.StartDate <= DateTime.UtcNow)
            .Paging(request.PageIndex, request.PageSize)
            .GetAllEventResponse(_httpResolver.LanguageKey())
            .ToListAsync(cancellationToken: cancellationToken);

    public async Task<Operation<List<GetAllAvailableEventResponse>>> HandleAsync(GetAllAvailableEventQuery request, CancellationToken cancellationToken = default)
    {
        return await _repository.Query<Event>()
            .FilterIgnoreSoftDeleted()
            .Paging(request.PageIndex, request.PageSize)
            .OrderBy(e => e.StartDate)
            .GetAllAvailableEventResponse(_httpResolver.LanguageKey(), _httpResolver.CurrentUserId())
            .ToListAsync(cancellationToken: cancellationToken);
    }
}