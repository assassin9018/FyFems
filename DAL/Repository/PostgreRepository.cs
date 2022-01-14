using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository;

public class PostgreRepository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
{
    private readonly MyFemsDbContext _dbContext;
    private readonly DbSet<TEntity> dbSet;

    public PostgreRepository(MyFemsDbContext dbContext)
    {
        _dbContext = dbContext;
        dbSet = dbContext.Set<TEntity>();
    }

    #region Create
    public void Save(TEntity entity)
        => dbSet.Add(entity);

    public void SaveAll(IEnumerable<TEntity> entities)
        => dbSet.AddRange(entities);

    public Task SaveAsync(TEntity entity)
        => Task.Run(() => Save(entity));

    public Task SaveAllAsync(IEnumerable<TEntity> entities)
        => Task.Run(() => SaveAll(entities));
    #endregion

    #region Update
    public void Update(TEntity entity)
        => dbSet.Update(entity);

    public void UpdateAll(IEnumerable<TEntity> entities)
        => dbSet.UpdateRange(entities);

    public Task UpdateAsync(TEntity entity)
        => Task.Run(() => Update(entity));

    public Task UpdateAllAsync(IEnumerable<TEntity> entities)
        => Task.Run(() => UpdateAll(entities));

    #endregion

    #region Delete

    public void Delete(object id)
        => Delete(Find(id));

    public async Task DeleteAsync(object id)
    {
        var value = await FindAsync(id);
        await DeleteAsync(value);
    }

    public void Delete(TEntity entity)
        => dbSet.Remove(entity);

    public Task DeleteAsync(TEntity entity)
        => Task.Run(() => Delete(entity));

    public void Delete(IEnumerable<TEntity> entities)
    {
        foreach (var item in entities)
            dbSet.Remove(item);
    }

    public Task DeleteAsync(IEnumerable<TEntity> entities)
        => Task.Run(() => Delete(entities));

    #endregion

    #region Get

    public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = dbSet.AsQueryable();
        if (filter != null)
            query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);

        if (orderBy != null)
            query = orderBy(query);

        return query.ToList();
    }

    public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
        => Task.Run(() => Get(filter, orderBy, includeProperties));

    #endregion

    #region Find

    public TEntity? Find(object id)
        => dbSet.Find(id);

    public Task<TEntity?> FindAsync(object id)
        => Task.Run(() => Find(id));

    #endregion
}
