using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository.EfCore;

/// <summary>
/// NaviItem repository implementation sử dụng Entity Framework
/// </summary>
public class NaviItemRepository : EfRepositoryBase<NaviItem>, INaviItemRepository
{
    public NaviItemRepository(RepositoryContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NaviItem>> GetAllActiveItemsAsync(bool trackChanges = false)
    {
        var query = _dbSet.Where(i => !i.IsDelete);
        
        return trackChanges
            ? await query.ToListAsync()
            : await query.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<NaviItem?> GetItemByIdAsync(int itemId, bool trackChanges = false)
    {
        var query = _dbSet.Where(i => i.Id == itemId && !i.IsDelete);
        
        return trackChanges
            ? await query.FirstOrDefaultAsync()
            : await query.AsNoTracking().FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<NaviItem?> GetItemWithProductItemsAsync(int itemId, bool trackChanges = false)
    {
        var query = _dbSet
            .Include(i => i.ProductItems!.Where(pi => !pi.IsDelete))
                .ThenInclude(pi => pi.Product)
            .Where(i => i.Id == itemId && !i.IsDelete);
        
        return trackChanges
            ? await query.FirstOrDefaultAsync()
            : await query.AsNoTracking().FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NaviItem>> GetItemsByTypeAsync(string type, bool trackChanges = false)
    {
        var query = _dbSet.Where(i => !i.IsDelete && i.Type == type);
        
        return trackChanges
            ? await query.ToListAsync()
            : await query.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NaviItem>> SearchItemsAsync(string searchTerm, bool trackChanges = false)
    {
        var query = _dbSet.Where(i => !i.IsDelete && 
            (i.Description != null && i.Description.Contains(searchTerm) ||
             i.Note != null && i.Note.Contains(searchTerm)));
        
        return trackChanges
            ? await query.ToListAsync()
            : await query.AsNoTracking().ToListAsync();
    }
}
