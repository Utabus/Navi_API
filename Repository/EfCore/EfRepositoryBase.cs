using System.Linq.Expressions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository.EfCore;

/// <summary>
/// Generic Entity Framework repository với CRUD operations cơ bản
/// Kế thừa class này để tái sử dụng tất cả CRUD operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public abstract class EfRepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly RepositoryContext _context;
    protected readonly DbSet<T> _dbSet;

    protected EfRepositoryBase(RepositoryContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false) =>
        trackChanges
            ? await _dbSet.ToListAsync()
            : await _dbSet.AsNoTracking().ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges = false) =>
        trackChanges
            ? await _dbSet.Where(expression).ToListAsync()
            : await _dbSet.Where(expression).AsNoTracking().ToListAsync();

    /// <inheritdoc/>
    public async Task<T?> GetByIdAsync(Guid id, bool trackChanges = false) =>
        trackChanges
            ? await _dbSet.FindAsync(id)
            : await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);

    /// <inheritdoc/>
    public void Create(T entity) => _dbSet.Add(entity);

    /// <inheritdoc/>
    public void Update(T entity) => _dbSet.Update(entity);

    /// <inheritdoc/>
    public void Delete(T entity) => _dbSet.Remove(entity);

    /// <inheritdoc/>
    public void CreateRange(IEnumerable<T> entities) => _dbSet.AddRange(entities);

    /// <inheritdoc/>
    public void DeleteRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
}
