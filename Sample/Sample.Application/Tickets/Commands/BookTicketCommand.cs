using Microsoft.EntityFrameworkCore;
using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.Core.Abstractions;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;
using NoResponse = Neptunee.OperationResponse.NoResponse;

namespace Sample.Application.Tickets.Commands;

public record BookTicketCommand(Guid EventId) : INeptuneeRequest<Operation<NoResponse>>;

internal class BookTicketCommandHandler : INeptuneeRequestHandler<BookTicketCommand, Operation<NoResponse>>
{
    private readonly IRepository _repository;
    private readonly IHttpResolver _httpResolver;

    public BookTicketCommandHandler(IRepository repository, IHttpResolver httpResolver)
    {
        _repository = repository;
        _httpResolver = httpResolver;
    }

    public async Task<Operation<NoResponse>> HandleAsync(BookTicketCommand request, CancellationToken cancellationToken = default)
    {
        var eventEntity = await _repository
            .TrackingQuery<Event>()
            .Where(request.EventId)
            .Include(e => e.Tickets)
            .FirstAsync(cancellationToken: cancellationToken);
        var result = eventEntity.Book(_httpResolver.CurrentUserId());
        if (result.IsFailure)
        {
            return result;
        }

        // await Task.Delay(10000); to testing Concurrency
        try
        {
            await _repository.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return await HandleAsync(request);
        }

        return Operation<NoResponse>.Ok();
    }

  
}