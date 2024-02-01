using System.Net;
using Microsoft.EntityFrameworkCore;
using Neptunee.Clock;
using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.Core.Abstractions;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;

namespace Sample.Application.Tickets.Commands;

public record CancelTicketCommand(Guid Id) : INeptuneeRequest<Operation<NoResponse>>;

internal  class CancelTicketCommandHandler : INeptuneeRequestHandler<CancelTicketCommand, Operation<NoResponse>>
{
    private readonly IRepository _repository;
    private readonly IHttpResolver _httpResolver;

    private readonly INeptuneeClock _clock;

    public CancelTicketCommandHandler(IRepository repository, IHttpResolver httpResolver, INeptuneeClock clock)
    {
        _repository = repository;
        _httpResolver = httpResolver;
        _clock = clock;
    }

    public async Task<Operation<NoResponse>> HandleAsync(CancelTicketCommand request, CancellationToken cancellationToken = default)
    {
        var operation = Operation<NoResponse>.Unknown();
        var currentUserId = _httpResolver.CurrentUserId();
        var deletedRows = await _repository
            .TrackingQuery<Ticket>()
            .Where(t => t.Id == request.Id && t.ParticipationUserId == currentUserId)
            .ExecuteUpdateAsync(e => e.SetProperty(f => f.UtcDateDeleted, _ => _clock.UtcNow), cancellationToken: cancellationToken);

        return deletedRows == 0
            ? operation.SetStatusCode(HttpStatusCode.NotFound)
            : operation.SetStatusCode(HttpStatusCode.OK);
    }
}