using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.Core.Abstractions.Security;
using Sample.Application.Core.Shared.Handlers;
using Sample.Application.ParticipationUsers.Commands.Responses;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;

namespace Sample.Application.ParticipationUsers.Commands;

public record LoginParticipationUserCommand(string Email, string Password) : INeptuneeRequest<Operation<AuthorizedParticipationUserResponse>>;

internal class LoginParticipationUserCommandHandler : BaseSecurityHandler<LoginParticipationUserCommand, Operation<AuthorizedParticipationUserResponse>>
{
    public LoginParticipationUserCommandHandler(IPasswordHasher passwordHasher, IRepository repository, IJwtBearerGenerator jwtBearerGenerator) : base(passwordHasher, repository, jwtBearerGenerator)
    {
    }

    public override async Task<Operation<AuthorizedParticipationUserResponse>> HandleAsync(LoginParticipationUserCommand request, CancellationToken cancellationToken = default)
    {
        var result = await Login<ParticipationUser>(request.Email, request.Password, cancellationToken);
        return result.IsFailure ? result : new AuthorizedParticipationUserResponse(result);
    }
}