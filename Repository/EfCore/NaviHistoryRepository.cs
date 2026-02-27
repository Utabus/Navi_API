using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository.EfCore;

/// <summary>
/// NaviHistory repository implementation sử dụng Entity Framework
/// </summary>
public class NaviHistoryRepository : EfRepositoryBase<NaviHistory>, INaviHistoryRepository
{
    public NaviHistoryRepository(RepositoryContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NaviHistory>> GetAllActiveHistoriesAsync(bool trackChanges = false)
    {
        var query = _dbSet
            .Where(h => !h.IsDelete)
            .OrderByDescending(h => h.CDT);

        return trackChanges
            ? await query.ToListAsync()
            : await query.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<NaviHistory?> GetHistoryByIdAsync(int id, bool trackChanges = false)
    {
        var query = _dbSet
            .Include(h => h.ProductItem)
            .Where(h => h.Id == id && !h.IsDelete);

        return trackChanges
            ? await query.FirstOrDefaultAsync()
            : await query.AsNoTracking().FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NaviHistory>> GetHistoriesByCodeNVAsync(string codeNV, bool trackChanges = false)
    {
        var query = _dbSet
            .Where(h => h.CodeNV == codeNV && !h.IsDelete)
            .OrderByDescending(h => h.CDT);

        return trackChanges
            ? await query.ToListAsync()
            : await query.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NaviHistory>> GetHistoriesByProductItemIdAsync(int productItemId, bool trackChanges = false)
    {
        var query = _dbSet
            .Where(h => h.PRODUCT_ITEM_Id == productItemId && !h.IsDelete)
            .OrderByDescending(h => h.CDT);

        return trackChanges
            ? await query.ToListAsync()
            : await query.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NaviHistory>> GetHistoriesByPOAsync(string po, bool trackChanges = false)
    {
        var query = _dbSet
            .Where(h => h.PO == po && !h.IsDelete)
            .OrderByDescending(h => h.CDT);

        return trackChanges
            ? await query.ToListAsync()
            : await query.AsNoTracking().ToListAsync();
    }
}
