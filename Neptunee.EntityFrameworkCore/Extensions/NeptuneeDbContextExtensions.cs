using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Neptunee.Clock;
using Neptunee.Entities;

namespace Microsoft.EntityFrameworkCore;

public static class NeptuneeDbContextExtensions
{
    public static IQueryable<TEntity> TrackingQuery<TEntity>(this INeptuneeDbContext dbContext) where TEntity : class, INeptuneeEntity
    {
        return dbContext.Set<TEntity>();
    }

    public static IQueryable<TEntity> TrackingQuery<TEntity>(this DbContext dbContext) where TEntity : class, INeptuneeEntity
    {
        return dbContext.Set<TEntity>();
    }

    public static IQueryable<TEntity> Query<TEntity>(this INeptuneeDbContext dbContext) where TEntity : class, INeptuneeEntity
    {
        return dbContext.TrackingQuery<TEntity>().AsNoTracking();
    }

    public static IQueryable<TEntity> Query<TEntity>(this DbContext dbContext) where TEntity : class, INeptuneeEntity
    {
        return dbContext.TrackingQuery<TEntity>().AsNoTracking();
    }

    public static EntityEntry<TEntity> Add<TEntity>(this DbContext dbContext,TEntity entity) where TEntity : class, INeptuneeCreatableEntity
    {
        entity.UtcDateCreated = dbContext.GetService<INeptuneeClock>().UtcNow;
        return dbContext.Add(entity);
    }  
    public static EntityEntry<TEntity> Add<TEntity>(this DbSet<TEntity> dbSet,TEntity entity) where TEntity : class, INeptuneeCreatableEntity
    {
        entity.UtcDateCreated = dbSet.GetService<INeptuneeClock>().UtcNow;
        return dbSet.Add(entity);
    }

    

    public static void SoftDelete<TEntity>(this DbSet<TEntity> dbSet, TEntity entity) where TEntity : class, INeptuneeDeletableEntity
    {
        entity.UtcDateDeleted = dbSet.GetService<INeptuneeClock>().UtcNow;
    }

    public static void SoftDeleteRange<TEntity>(this DbSet<TEntity> dbSet, IEnumerable<TEntity> entities) where TEntity : class, INeptuneeDeletableEntity
    {
        foreach (var entity in entities)
        {
            dbSet.SoftDelete(entity);
        }
    }
}