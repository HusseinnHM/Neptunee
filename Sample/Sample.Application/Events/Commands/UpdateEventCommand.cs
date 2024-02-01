using System.Net;
using Microsoft.EntityFrameworkCore;
using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.Core.Abstractions;
using Sample.Application.Events.Queries.QueryObjects;
using Sample.Application.Events.Queries.Responses;
using Sample.Domain;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;

namespace Sample.Application.Events.Commands;

public record UpdateEventCommand(
    Guid Id,
    string Name,
    string Description,
    string Location,
    DateTime StartDate,
    DateTime EndDate,
    int AvailableTickets) : INeptuneeRequest<Operation<GetAllEventResponse>>;

internal class UpdateEventCommandHandler : INeptuneeRequestHandler<UpdateEventCommand, Operation<GetAllEventResponse>>
{
    private readonly IRepository _repository;
    private readonly IHttpResolver _httpResolver;

    public UpdateEventCommandHandler(IRepository repository, IHttpResolver httpResolver)
    {
        _repository = repository;
        _httpResolver = httpResolver;
    }

    public async Task<Operation<GetAllEventResponse>> HandleAsync(UpdateEventCommand command, CancellationToken cancellationToken = default)
    {
        var operation = Operation<GetAllEventResponse>.Unknown();

        operation.OrFailureIf(command.StartDate < DateTime.Now, Errors.Events.StartDateCannotBeInPast);
        operation.OrFailureIf(command.EndDate < command.StartDate, Errors.Events.EndDateCannotBeBeforeStatDate);
        // other validations checks ..

        if (operation.IsFailure)
        {
            return operation;
        }

        var eventEntity = await _repository.TrackingQuery<Event>().Where(e => e.Id == command.Id).Include(e => e.Tickets).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (eventEntity is null)
        {
            return operation.SetStatusCode(HttpStatusCode.NotFound);
        }

        if (eventEntity.EventManagerId != _httpResolver.CurrentUserId())
        {
            return operation.SetStatusCode(HttpStatusCode.Forbidden);
        }

        var result = eventEntity.Modify(command.Name,
            command.Description,
            command.Location,
            command.StartDate,
            command.EndDate,
            command.AvailableTickets,
            _httpResolver.LanguageKey());
        if (result.IsFailure)
        {
            return result;
        }

        // await Task.Delay(10000); to testing Concurrency
        try
        {
            await _repository.SaveChangesAsync(cancellationToken);
            operation.SetResponse(await _repository.Query<Event>().Where(eventEntity.Id).GetAllEventResponse(_httpResolver.LanguageKey()).FirstAsync(cancellationToken: cancellationToken));
        }
        catch (DbUpdateConcurrencyException)
        {
            operation.Error(Errors.Events.ChangesHappened);
        }

        return operation;
    }
}