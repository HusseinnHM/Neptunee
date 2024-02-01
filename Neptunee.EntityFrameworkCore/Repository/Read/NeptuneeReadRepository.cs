using Microsoft.EntityFrameworkCore;
using Neptunee.Entities;

namespace Neptunee.EntityFrameworkCore.Repository;

public class NeptuneeReadRepository<TIContext> : INeptuneeReadRepository
    where TIContext : INeptuneeDbContext

{
    protected readonly TIContext Context;

    public NeptuneeReadRepository(TIContext context)
    {
        Context = context;
    }

    public virtual IQueryable<TEntity> Query<TEntity>() where TEntity : class, INeptuneeEntity
    {
        return Context.Query<TEntity>();
    }

    public virtual IQueryable<TEntity> TrackingQuery<TEntity>() where TEntity : class, INeptuneeEntity
    {
        return Context.TrackingQuery<TEntity>();
    }



  
}