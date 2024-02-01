using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.Core.Abstractions.Security;
using Sample.Application.Core.Shared.Handlers;
using Sample.Application.ParticipationUsers.Commands.Responses;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;

namespace Sample.Application.ParticipationUsers.Commands;

public record RegisterParticipationUserCommand(string Name, string Email, string Password) : INeptuneeRequest<Operation<AuthorizedParticipationUserResponse>>;

internal class RegisterParticipationUserCommandHandler : BaseSecurityHandler<RegisterParticipationUserCommand, Operation<AuthorizedParticipationUserResponse>>
{
    public RegisterParticipationUserCommandHandler(IPasswordHasher passwordHasher, IRepository repository, IJwtBearerGenerator jwtBearerGenerator) : base(passwordHasher, repository, jwtBearerGenerator)
    {
    }

    public override async Task<Operation<AuthorizedParticipationUserResponse>> HandleAsync(RegisterParticipationUserCommand request, CancellationToken cancellationToken = default)
    {
        var result = await Register(request.Email,
            () => new ParticipationUser(request.Name, request.Email, PasswordHasher.HashPassword(request.Password)),
            cancellationToken);
        return result.IsFailure ? result : new AuthorizedParticipationUserResponse(result);
    }
}