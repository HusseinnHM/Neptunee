using Microsoft.AspNetCore.Mvc;
using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.Events.Queries.Responses;
using Sample.Application.Tickets.Commands;
using Sample.Application.Tickets.Queries;
using Sample.Application.Tickets.Queries.Responses;
using Sample.Infrastructure.Security.Policies.UserType;
using Sample.SharedKernel;
using Swashbuckle.AspNetCore.Annotations;

namespace Sample.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TicketsController : ControllerBase
{
    [HttpGet]
    [HasUserTypes(ConstValues.UserTypes.ParticipationUser)]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<GetAllEventResponse>))]
    public async Task<IActionResult> GetAll(
        [FromServices] INeptuneeRequestHandler<GetAllTicketQuery, Operation<List<GetAllTicketResponse>>> handler,
        [FromQuery] GetAllTicketQuery query,
        CancellationToken ct)
        => await handler.HandleAsync(query, ct).ToIActionResultAsync();

    [HttpPost]
    [HasUserTypes(ConstValues.UserTypes.ParticipationUser)]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Operation<NoResponse>))]
    public async Task<IActionResult> Book(
        [FromServices] INeptuneeRequestHandler<BookTicketCommand, Operation<NoResponse>> handler,
        [FromBody] BookTicketCommand command,
        CancellationToken ct)
        => await handler.HandleAsync(command, ct).ToIActionResultAsync();

    [HttpPost]
    [HasUserTypes(ConstValues.UserTypes.ParticipationUser)]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Operation<NoResponse>))]
    public async Task<IActionResult> Cancel(
        [FromServices] INeptuneeRequestHandler<CancelTicketCommand, Operation<NoResponse>> handler,
        [FromBody] CancelTicketCommand command,
        CancellationToken ct)
        => await handler.HandleAsync(command, ct).ToIActionResultAsync();
}