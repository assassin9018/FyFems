using DAL.Models;
using System.Linq.Expressions;

namespace DAL.Repository;

public interface IRepository<TEntity> where TEntity : EntityBase
{
    void Delete(IEnumerable<TEntity> entities);
    Task DeleteAsync(IEnumerable<TEntity> entities);
    TEntity? Find(object id);
    Task<TEntity?> FindAsync(object id);
    IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");
    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");
    void Save(TEntity entity);
    void SaveAll(IEnumerable<TEntity> entities);
    Task SaveAllAsync(IEnumerable<TEntity> entities);
    Task SaveAsync(TEntity entity);
    void Update(TEntity entity);
    void UpdateAll(IEnumerable<TEntity> entities);
    Task UpdateAllAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
}
