using System.Linq.Expressions;
using Neptunee.Entities;

namespace Neptunee.EntityFrameworkCore.Specification;

public abstract class NeptuneeSpecification<TEntity> : INeptuneeSpecification<TEntity> where TEntity : class, INeptuneeEntity
{
    protected NeptuneeSpecification(Expression<Func<TEntity, bool>> filter) : this()
    {
        Filters.Add(filter);
    }

    protected NeptuneeSpecification()
    {
        Filters = new();
    }

    public List<Expression<Func<TEntity, bool>>> Filters { get; private set; }
    public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }
    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
    public Expression<Func<TEntity, object>>? ThenOrderByDescending { get; private set; }
    public Expression<Func<TEntity, object>>? ThenOrderBy { get; private set; }
    public Expression<Func<TEntity, object>>? GroupBy { get; private set; }

    public List<Expression<Func<TEntity, object>>> Includes { get; } = new();
    public List<string> IncludeStrings { get; } = new();
    public int? Take { get; private set; }
    public int? Skip { get; private set; }
    public bool Tracking { get; private set; } = false;
    public bool SplitQuery { get; private set; } = false;

    protected virtual void AddFilter(Expression<Func<TEntity, bool>> filterExpression)
    {
        Filters.Add(filterExpression);
    }

    protected virtual void AddFilterIf(bool condition, Expression<Func<TEntity, bool>> filterExpression)
    {
        if (condition)
        {
            AddFilter(filterExpression);
        }
    }

    protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected virtual void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    protected virtual void ApplyPaging(int pageIndex, int size)
    {
        if (pageIndex <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageIndex), "must greater than 0");
        }

        Skip = (pageIndex - 1) * size;
        Take = size;
    }

    protected virtual void ApplyTake(int take)
    {
        Take = take;
    }

    protected virtual void ApplySkip(int skip)
    {
        Skip = skip;
    }

    protected virtual void ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected virtual void ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    protected virtual void ApplyThenOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        ThenOrderBy = orderByExpression;
    }

    protected virtual void ApplyThenOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
    {
        ThenOrderByDescending = orderByDescendingExpression;
    }

    protected virtual void ApplyGroupBy(Expression<Func<TEntity, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }

    protected virtual void AsTracking()
    {
        Tracking = true;
    }

    protected virtual void AsSplitQuery()
    {
        SplitQuery = true;
    }
}