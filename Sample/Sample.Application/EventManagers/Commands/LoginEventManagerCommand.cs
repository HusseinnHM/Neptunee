using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.Core.Abstractions.Security;
using Sample.Application.Core.Shared.Handlers;
using Sample.Application.EventManagers.Commands.Responses;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;

namespace Sample.Application.EventManagers.Commands;

public record LoginEventManagerCommand(string Email, string Password) : INeptuneeRequest<Operation<AuthorizedEventManagerResponse>>;

internal class LoginEventManagerCommandHandler : BaseSecurityHandler<LoginEventManagerCommand, Operation<AuthorizedEventManagerResponse>>
{
    public LoginEventManagerCommandHandler(IPasswordHasher passwordHasher, IRepository repository, IJwtBearerGenerator jwtBearerGenerator) : base(passwordHasher, repository, jwtBearerGenerator)
    {
    }

    public override async Task<Operation<AuthorizedEventManagerResponse>> HandleAsync(LoginEventManagerCommand request, CancellationToken cancellationToken = default)
    {
        var result = await Login<EventManager>(request.Email, request.Password, cancellationToken);
        return result.IsFailure ? result : new AuthorizedEventManagerResponse(result);
    }
}