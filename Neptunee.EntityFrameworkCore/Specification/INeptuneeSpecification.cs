using System.Linq.Expressions;
using Neptunee.Entities;

namespace Neptunee.EntityFrameworkCore.Specification;

public interface INeptuneeSpecification<TEntity> where TEntity : class, INeptuneeEntity
{
    public List<Expression<Func<TEntity, bool>>> Filters { get; }
    public List<Expression<Func<TEntity, object>>> Includes { get; }
    public List<string> IncludeStrings { get; }
    public Expression<Func<TEntity, object>>? OrderBy { get; }
    public Expression<Func<TEntity, object>>? OrderByDescending { get; }
    public Expression<Func<TEntity, object>>? ThenOrderBy { get; }
    public Expression<Func<TEntity, object>>? ThenOrderByDescending { get; }
    public Expression<Func<TEntity, object>>? GroupBy { get; }

    public int? Take { get; }
    public int? Skip { get; }
    public bool Tracking { get; }
    public bool SplitQuery { get; }
}