using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.Core.Abstractions.Security;
using Sample.Application.Core.Shared.Handlers;
using Sample.Application.EventManagers.Commands.Responses;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;

namespace Sample.Application.EventManagers.Commands;

public record RegisterEventManagerCommand(string Name, string Email, string Password) : INeptuneeRequest<Operation<AuthorizedEventManagerResponse>>;

internal class RegisterEventManagerCommandHandler : BaseSecurityHandler<RegisterEventManagerCommand, Operation<AuthorizedEventManagerResponse>>
{
    public RegisterEventManagerCommandHandler(IPasswordHasher passwordHasher, IRepository repository, IJwtBearerGenerator jwtBearerGenerator) : base(passwordHasher, repository, jwtBearerGenerator)
    {
    }

    public override async Task<Operation<AuthorizedEventManagerResponse>> HandleAsync(RegisterEventManagerCommand request, CancellationToken cancellationToken = default)
    {
        var result = await Register(request.Email,
            () => new EventManager(request.Name, request.Email, PasswordHasher.HashPassword(request.Password)),
            cancellationToken);
        return result.IsFailure ? result : new AuthorizedEventManagerResponse(result);
    }
}