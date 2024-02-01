using Microsoft.EntityFrameworkCore;
using Neptunee.Entities;

namespace Neptunee.EntityFrameworkCore.Specification;

internal class NeptuneeSpecificationEvaluator<TEntity> where TEntity : class, INeptuneeEntity
{
    public static async Task<List<TEntity>> GetList(IQueryable<TEntity> query, INeptuneeSpecification<TEntity> specification)
    {
        return await GetQuery(query, specification).ToListAsync();
    }

    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> query, INeptuneeSpecification<TEntity> specification)
    {
        query = specification.Filters.Aggregate(query, (queryable, predicate) => queryable.Where(predicate));

        query = specification.Includes.Aggregate(query, (queryable, include) => queryable.Include(include));

        query = specification.IncludeStrings.Aggregate(query, (queryable, include) => queryable.Include(include));

        query = specification switch
        {
            { OrderBy: not null, ThenOrderBy: not null } => query.OrderBy(specification.OrderBy).ThenBy(specification.ThenOrderBy),
            { OrderBy: not null, ThenOrderByDescending: not null } => query.OrderBy(specification.OrderBy).ThenByDescending(specification.ThenOrderByDescending),
            { OrderBy: not null } => query.OrderBy(specification.OrderBy),
            { OrderByDescending: not null, ThenOrderBy: not null } => query.OrderByDescending(specification.OrderByDescending).ThenBy(specification.ThenOrderBy),
            { OrderByDescending: not null, ThenOrderByDescending: not null } => query.OrderByDescending(specification.OrderByDescending).ThenByDescending(specification.ThenOrderByDescending),
            { OrderByDescending: not null } => query.OrderByDescending(specification.OrderByDescending),
            _ => query
        };


        if (specification.GroupBy != null)
        {
            query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
        }

        if (specification.Skip is not (null or 0))
        {
            query = query.Skip(specification.Skip.Value);
        }

        if (specification.Take is not (null or 0))
        {
            query = query.Take(specification.Take.Value);
        }

        if (specification.SplitQuery)
        {
            query = query.AsSplitQuery();
        }

        return specification.Tracking ? query.AsTracking() : query.AsNoTracking();
    }
}