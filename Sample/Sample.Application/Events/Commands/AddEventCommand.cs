using Microsoft.EntityFrameworkCore;
using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.Core.Abstractions;
using Sample.Application.Events.Queries.QueryObjects;
using Sample.Application.Events.Queries.Responses;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;

namespace Sample.Application.Events.Commands;

public record AddEventCommand(
    string Name,
    string Description,
    string Location,
    DateTime StartDate,
    DateTime EndDate,
    int AvailableTickets) : INeptuneeRequest<Operation<GetAllEventResponse>>;

internal class AddEventCommandHandler : INeptuneeRequestHandler<AddEventCommand, Operation<GetAllEventResponse>>
{
    private readonly IRepository _repository;
    private readonly IHttpResolver _httpResolver;
    private readonly IFakeTranslate _fakeTranslate;

    public AddEventCommandHandler(IRepository repository, IHttpResolver httpResolver, IFakeTranslate fakeTranslate)
    {
        _repository = repository;
        _httpResolver = httpResolver;
        _fakeTranslate = fakeTranslate;
    }

    public async Task<Operation<GetAllEventResponse>> HandleAsync(AddEventCommand request, CancellationToken cancellationToken = default)
    {
        var eventEntity = new Event(request.Name,
            request.Description,
            request.Location,
            request.StartDate,
            request.EndDate,
            request.AvailableTickets,
            _httpResolver.CurrentUserId(),
            _httpResolver.LanguageKey());
        
        _fakeTranslate.Translate(eventEntity.Name, eventEntity.Description, eventEntity.Location);
        _repository.Add(eventEntity);
        await _repository.SaveChangesAsync(cancellationToken);
        
        return await _repository.Query<Event>()
            .Where(eventEntity.Id)
            .GetAllEventResponse(_httpResolver.LanguageKey())
            .FirstAsync(cancellationToken: cancellationToken);
    }
}