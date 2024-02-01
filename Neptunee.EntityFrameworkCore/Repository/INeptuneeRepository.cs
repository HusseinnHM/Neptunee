using Neptunee.Entities;

namespace Neptunee.EntityFrameworkCore.Repository;

public interface INeptuneeRepository : INeptuneeReadRepository
{
    void Add<TEntity>(TEntity entity) where TEntity : class, INeptuneeEntity;
    void Update<TEntity>(TEntity entity) where TEntity : class, INeptuneeEntity;
    void Remove<TEntity>(TEntity entity) where TEntity : class, INeptuneeEntity;
    void SoftDelete<TEntity>(TEntity entity) where TEntity : class, INeptuneeDeletableEntity;
    void SoftDeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, INeptuneeDeletableEntity;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}