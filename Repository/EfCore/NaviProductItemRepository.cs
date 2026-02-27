using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository.EfCore;

/// <summary>
/// NaviProductItem repository implementation sử dụng Entity Framework
/// </summary>
public class NaviProductItemRepository : EfRepositoryBase<NaviProductItem>, INaviProductItemRepository
{
    public NaviProductItemRepository(RepositoryContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NaviProductItem>> GetAllActiveProductItemsAsync(bool trackChanges = false)
    {
        var query = _dbSet.Where(pi => !pi.IsDelete);
        
        return trackChanges
            ? await query.ToListAsync()
            : await query.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<NaviProductItem?> GetProductItemByIdAsync(int productItemId, bool trackChanges = false)
    {
        var query = _dbSet.Where(pi => pi.Id == productItemId && !pi.IsDelete);
        
        return trackChanges
            ? await query.FirstOrDefaultAsync()
            : await query.AsNoTracking().FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NaviProductItem>> GetItemsByProductIdAsync(int productId, bool trackChanges = false)
    {
        var query = _dbSet
            .Include(pi => pi.Item)
            .Where(pi => pi.ProductId == productId && !pi.IsDelete && 
                         (pi.Item == null || !pi.Item.IsDelete));
        
        return trackChanges
            ? await query.ToListAsync()
            : await query.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NaviProductItem>> GetProductsByItemIdAsync(int itemId, bool trackChanges = false)
    {
        var query = _dbSet
            .Include(pi => pi.Product)
            .Where(pi => pi.ItemId == itemId && !pi.IsDelete && 
                         (pi.Product == null || !pi.Product.IsDelete));
        
        return trackChanges
            ? await query.ToListAsync()
            : await query.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> ProductItemExistsAsync(int productId, int itemId)
    {
        return await _dbSet.AnyAsync(pi => 
            pi.ProductId == productId && 
            pi.ItemId == itemId && 
            !pi.IsDelete);
    }

    /// <inheritdoc/>
    public async Task<NaviProductItem?> GetProductItemWithDetailsAsync(int productItemId, bool trackChanges = false)
    {
        var query = _dbSet
            .Include(pi => pi.Product)
            .Include(pi => pi.Item)
            .Where(pi => pi.Id == productItemId && !pi.IsDelete &&
                         (pi.Product == null || !pi.Product.IsDelete) &&
                         (pi.Item == null || !pi.Item.IsDelete));
        
        return trackChanges
            ? await query.FirstOrDefaultAsync()
            : await query.AsNoTracking().FirstOrDefaultAsync();
    }
}
