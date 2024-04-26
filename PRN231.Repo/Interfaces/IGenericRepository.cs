using System.Linq.Expressions;

namespace PRN231.Repo.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "",
        int? pageIndex = null,
        int? pageSize = null);

    Task<TEntity> GetByIDAsync(object id);
    bool isExists(Expression<Func<TEntity, bool>> filter);
    void Insert(TEntity entity);
    Task InsertRangeAsync(IEnumerable<TEntity> entities);
    void Delete(object id);
    void Delete(TEntity entityToDelete);
    void Update(TEntity entityToUpdate);
    void UpdateRange(IEnumerable<TEntity> entities);
    Task<TEntity> FindUserAsync(Expression<Func<TEntity, bool>> filter);
}