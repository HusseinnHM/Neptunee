using Sample.Domain.Entities;

namespace Sample.Application.Core.Shared.QueryObjects;

public static class UserFilters
{
    public static IQueryable<TUser> FindBy<TUser>(this IQueryable<TUser> query, string email) where TUser : class, IEntityHasEmail
    {
        return query.Where(u => u.Email.ToUpper() == email.ToUpper());
    }
}