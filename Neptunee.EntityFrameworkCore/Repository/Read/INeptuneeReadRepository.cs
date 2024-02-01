using Neptunee.Entities;

namespace Neptunee.EntityFrameworkCore.Repository;

public interface INeptuneeReadRepository 
{
    IQueryable<TEntity> Query<TEntity>() where TEntity : class, INeptuneeEntity;
    IQueryable<TEntity> TrackingQuery<TEntity>() where TEntity : class, INeptuneeEntity;
}