using System.Net;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Neptunee.Entities;
using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.Core.Abstractions.Security;
using Sample.Application.Core.Shared.QueryObjects;
using Sample.Domain;
using Sample.Domain.Entities;
using Sample.Domain.Repositories;
using Sample.SharedKernel;

namespace Sample.Application.Core.Shared.Handlers;

internal abstract class BaseSecurityHandler<TRequest, TResponse> : INeptuneeRequestHandler<TRequest, TResponse> where TRequest : class, INeptuneeRequest<TResponse>
{
    protected readonly IPasswordHasher PasswordHasher;
    protected readonly IRepository Repository;
    protected readonly IJwtBearerGenerator JwtBearerGenerator;

    public BaseSecurityHandler(IPasswordHasher passwordHasher, IRepository repository, IJwtBearerGenerator jwtBearerGenerator)
    {
        PasswordHasher = passwordHasher;
        Repository = repository;
        JwtBearerGenerator = jwtBearerGenerator;
    }

    public abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);

    protected async Task<Result<string>> Login<TUser>(string email, string password, CancellationToken cancellationToken) where TUser : class, IEntityHasEmail, IEntityHasPassword, INeptuneeEntity<Guid>
    {
        var user = await Repository
            .Query<TUser>()
            .FindBy(email)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (user is null)
        {
            return Result.With(HttpStatusCode.NotFound, Errors.Users.EmailNotFound).To<string>();
        }

        if (!PasswordHasher.VerifyHashedPassword(user.PasswordHash, password))
        {
            return Result.With(HttpStatusCode.NotFound, Errors.Users.WrongEmailOrPassword).To<string>();
        }


        return Success(user);
    }

    protected async Task<Result<string>> Register<TUser>(string email, Func<TUser> userFunc, CancellationToken cancellationToken) where TUser : class, IEntityHasEmail, IEntityHasPassword, INeptuneeEntity<Guid>
    {
        if (await Repository.Query<TUser>().FindBy(email).AnyAsync(cancellationToken: cancellationToken))
        {
            return Result.With(HttpStatusCode.BadRequest, Errors.Users.EmailAlreadyUsed).To<string>();
        }

        var user = userFunc();
        Repository.Add(user);
        await Repository.SaveChangesAsync(cancellationToken);
        return Success(user);
    }

    private string Success<TUser>(TUser user) where TUser : INeptuneeEntity<Guid>
        => JwtBearerGenerator.Generate(new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ConstValues.ClaimTypes.UserType, typeof(TUser).Name),
            // other claim like expire, email ...etc
        });
}