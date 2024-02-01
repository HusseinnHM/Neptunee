using Microsoft.EntityFrameworkCore;
using Neptunee.Entities;

namespace Neptunee.EntityFrameworkCore.Repository;

public class NeptuneeRepository<TIContext> : NeptuneeReadRepository<TIContext>, INeptuneeRepository
    where TIContext : INeptuneeDbContext
{
    public NeptuneeRepository(TIContext context) : base(context)
    {
    }


    public virtual void Add<TEntity>(TEntity entity) where TEntity : class, INeptuneeEntity
    {
        Context.Set<TEntity>().Add(entity);
    }

    public virtual void Update<TEntity>(TEntity entity) where TEntity : class, INeptuneeEntity
    {
        Context.Set<TEntity>().Update(entity);
    }

    public virtual void Remove<TEntity>(TEntity entity) where TEntity : class, INeptuneeEntity
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public virtual void SoftDelete<TEntity>(TEntity entity) where TEntity : class, INeptuneeDeletableEntity
    {
        Context.Set<TEntity>().SoftDelete(entity);
    }

    public virtual void SoftDeleteRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class, INeptuneeDeletableEntity
    {
        Context.Set<TEntity>().SoftDeleteRange(entities);
    }


    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await Context.SaveChangesAsync(cancellationToken);
    }
}