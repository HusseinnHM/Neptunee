
using Neptunee.Entities;
using Neptunee.EntityFrameworkCore.Specification;

namespace System.Linq;

public static class NeptuneeQueryableExtensions
{
    public static IQueryable<TEntity> Apply<TEntity>(this IQueryable<TEntity> query,
        INeptuneeSpecification<TEntity> specification) where TEntity : class, INeptuneeEntity =>
        NeptuneeSpecificationEvaluator<TEntity>.GetQuery(query, specification);
}