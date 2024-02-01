using Microsoft.AspNetCore.Mvc;
using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.API.Core.ActionsFilters;
using Sample.Application.Events.Commands;
using Sample.Application.Events.Queries;
using Sample.Application.Events.Queries.Responses;
using Sample.Infrastructure.Security.Policies.UserType;
using Sample.SharedKernel;
using Swashbuckle.AspNetCore.Annotations;

namespace Sample.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class EventsController : ControllerBase
{
    [HttpGet]
    [HasUserTypes(ConstValues.UserTypes.EventManager)]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<GetAllEventResponse>))]
    public async Task<IActionResult> GetAll(
        [FromServices] INeptuneeRequestHandler<GetAllEventQuery, Operation<List<GetAllEventResponse>>> handler,
        [FromQuery] GetAllEventQuery query,
        CancellationToken ct)
        => await handler.HandleAsync(query,ct).ToIActionResultAsync();

    [HttpGet]
    [HasUserTypes(ConstValues.UserTypes.ParticipationUser)]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<GetAllAvailableEventResponse>))]
    public async Task<IActionResult> GetAvailable(
        [FromServices] INeptuneeRequestHandler<GetAllAvailableEventQuery, Operation<List<GetAllAvailableEventResponse>>> handler,
        [FromQuery] GetAllAvailableEventQuery query,
        CancellationToken ct)
        => await handler.HandleAsync(query, ct).ToIActionResultAsync();

    [HttpPost]
    [HasUserTypes(ConstValues.UserTypes.EventManager)]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(GetAllEventResponse))]
    [TypeFilter(typeof(ForceDefaultLanguage))]
    public async Task<IActionResult> Add(
        [FromServices] INeptuneeRequestHandler<AddEventCommand, Operation<GetAllEventResponse>> handler,
        [FromBody] AddEventCommand command,
        CancellationToken ct)
        => await handler.HandleAsync(command, ct).ToIActionResultAsync();

    [HttpPost]
    [HasUserTypes(ConstValues.UserTypes.EventManager)]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(GetAllEventResponse))]
    public async Task<IActionResult> Update(
        [FromServices] INeptuneeRequestHandler<UpdateEventCommand, Operation<GetAllEventResponse>> handler,
        [FromBody] UpdateEventCommand command,
        CancellationToken ct)
        => await handler.HandleAsync(command, ct).ToIActionResultAsync();

    // [HttpDelete]
    // [HasUserTypes(ConstValues.UserTypes.EventManager)]
    // public async Task<IActionResult> Delete(
    //     [FromServices] INeptuneeRequestHandler<DeleteEventCommand, Operation<NoResponse>> handler,
    //     [FromBody] DeleteEventCommand command)
    //     => await handler.HandleAsync(command).ToIActionResultAsync();
}