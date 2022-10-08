using System.Linq.Expressions;
using Contracts.Common.Interfaces;
using Contracts.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Common;

public class RepositoryBaseAsync<T, K, TContext> : IRepositoryBaseAsync<T, K, TContext>
    where T : EntityBase<K>
    where TContext : DbContext
{
    private readonly TContext _dbContext;
    private readonly IUnitOfWork<TContext> _unitOfWork;
    public RepositoryBaseAsync(TContext dbContext, IUnitOfWork<TContext> unitOfWork)
    {
        _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
        _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(unitOfWork));
    }
    public Task<IDbContextTransaction> BeginTransactionAsync()
        => _dbContext.Database.BeginTransactionAsync();

    public async Task<K> CreateAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity.Id;
    }

    public async Task<IList<K>> CreateListAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        return entities.Select(x => x.Id).ToList();
    }

    public Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteListAsync(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public async Task EndTransactionAsync()
    {
        await SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
    }

    public IQueryable<T> FindAll(bool trackChanges = false)
        => !trackChanges ? _dbContext.Set<T>().AsNoTracking() : _dbContext.Set<T>();

    public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindAll(trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
        => !trackChanges
            ? _dbContext.Set<T>().Where(expression).AsNoTracking()
            : _dbContext.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    public async Task<T?> GetByIdAsync(K Id)
        => await FindByCondition(x => x.Id!.Equals(Id)).FirstOrDefaultAsync();

    public async Task<T?> GetByIdAsync(K Id, params Expression<Func<T, object>>[] includeProperties)
        => await FindByCondition(x => x.Id!.Equals(Id), trackChanges: false, includeProperties).FirstOrDefaultAsync();

    public Task RollbackTransactionAsync()
        => _dbContext.Database.RollbackTransactionAsync();

    public Task<int> SaveChangesAsync()
        => _unitOfWork.CommitAsync();

    public Task UpdateAsync(T entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Unchanged)
            return Task.CompletedTask;
        T exist = _dbContext.Set<T>().Find(entity.Id);
        _dbContext.Entry(exist).CurrentValues.SetValues(entity);
        return Task.CompletedTask;
    }

    public Task UpdateListAsync(IEnumerable<T> entities)
        => _dbContext.Set<T>().AddRangeAsync(entities);
}
