using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.ParticipationUsers.Commands;
using Sample.Application.ParticipationUsers.Commands.Responses;
using Sample.Application.ParticipationUsers.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace Sample.API.Controllers;

public sealed class ParticipationUsersController : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(AuthorizedParticipationUserResponse))]
    public async Task<IActionResult> Login(
        [FromServices] INeptuneeRequestHandler<LoginParticipationUserCommand, Operation<AuthorizedParticipationUserResponse>> handler,
        [FromBody] LoginParticipationUserCommand command,
        CancellationToken ct)
        => await handler.HandleAsync(command, ct).ToIActionResultAsync();

    [HttpPost]
    [AllowAnonymous]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(AuthorizedParticipationUserResponse))]
    public async Task<IActionResult> Register(
        [FromServices] INeptuneeRequestHandler<RegisterParticipationUserCommand, Operation<AuthorizedParticipationUserResponse>> handler,
        [FromBody] RegisterParticipationUserCommand command,
        CancellationToken ct)
        => await handler.HandleAsync(command, ct).ToIActionResultAsync();
}