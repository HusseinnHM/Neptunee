using System.Linq.Expressions;
using Neptunee.Entities;

namespace System.Linq;

public static class NeptuneeQueryableExtensions
{
    public static IQueryable<TEntity> WhereIf<TEntity>(
        this IQueryable<TEntity> query,
        bool condition,
        Expression<Func<TEntity, bool>> predicate) =>
        condition ? query.Where(predicate) : query;

    public static IQueryable<TEntity> OrderByDescendingIf<TEntity, TKey>(
        this IQueryable<TEntity> query,
        bool condition,
        Expression<Func<TEntity, TKey>> keySelector) =>
        condition ? query.OrderByDescending(keySelector) : query;

    public static IQueryable<TEntity> OrderByIf<TEntity, TKey>(
        this IQueryable<TEntity> query,
        bool condition,
        Expression<Func<TEntity, TKey>> keySelector,
        OrderByOption orderByOption = OrderByOption.Increasing) =>
        condition ? query.OrderBy(keySelector, orderByOption) : query;

    public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> queryable, params string[] includeProps) =>
        includeProps.Aggregate(queryable, (query, includeProp) => query.Include(includeProp));

    public static IQueryable<TEntity> Where<TEntity, TKey>(this IQueryable<TEntity> query, TKey id) where TEntity : class, INeptuneeEntity<TKey> where TKey : struct, IEquatable<TKey>
        => query.Where(entity => entity.Id.Equals(id));

    public static IQueryable<TEntity> Where<TEntity, TKey>(this IQueryable<TEntity> query, IEnumerable<TKey> ids) where TEntity : class, INeptuneeEntity<TKey> where TKey : struct, IEquatable<TKey>
        => query.Where(entity => ids.Contains(entity.Id));

    public static IQueryable<TEntity> FilterIgnoreSoftDeleted<TEntity>(this IQueryable<TEntity> query) where TEntity : class, INeptuneeDeletableEntity
        => query.Where(entity => !entity.UtcDateDeleted.HasValue);

    public static IQueryable<TEntity> Paging<TEntity>(this IQueryable<TEntity> query, int pageIndex, int size)
    {
        if (pageIndex <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageIndex), "must greater than 0");
        }

        return query.Skip((pageIndex - 1) * size).Take(size);
    }

    public static IQueryable<TEntity> OrderBy<TEntity, TKey>(this IQueryable<TEntity> query,
        Expression<Func<TEntity, TKey>> keySelector,
        OrderByOption orderByOption = OrderByOption.Increasing) =>
        orderByOption switch
        {
            OrderByOption.Increasing => query.OrderBy(keySelector),
            OrderByOption.Descending => query.OrderByDescending(keySelector),
            _ => throw new ArgumentOutOfRangeException(nameof(orderByOption), orderByOption, null)
        };
}

public enum OrderByOption
{
    Increasing,
    Descending
}