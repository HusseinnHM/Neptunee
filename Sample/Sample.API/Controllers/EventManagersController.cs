using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.EventManagers.Commands;
using Sample.Application.EventManagers.Commands.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace Sample.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public sealed class EventManagersController : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(AuthorizedEventManagerResponse))]
    public async Task<IActionResult> Login(
        [FromServices] INeptuneeRequestHandler<LoginEventManagerCommand, Operation<AuthorizedEventManagerResponse>> handler,
        [FromBody] LoginEventManagerCommand command,
        CancellationToken ct)
        => await handler.HandleAsync(command, ct).ToIActionResultAsync();

    [HttpPost]
    [AllowAnonymous]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(AuthorizedEventManagerResponse))]
    public async Task<IActionResult> Register(
        [FromServices] INeptuneeRequestHandler<RegisterEventManagerCommand, Operation<AuthorizedEventManagerResponse>> handler,
        [FromBody] RegisterEventManagerCommand command,
        CancellationToken ct)
        => await handler.HandleAsync(command, ct).ToIActionResultAsync();
}