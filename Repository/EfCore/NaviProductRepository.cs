using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository.EfCore;

/// <summary>
/// NaviProduct repository implementation sử dụng Entity Framework
/// </summary>
public class NaviProductRepository : EfRepositoryBase<NaviProduct>, INaviProductRepository
{
    public NaviProductRepository(RepositoryContext context) : base(context)
    {
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NaviProduct>> GetAllActiveProductsAsync(bool trackChanges = false)
    {
        var query = _dbSet.Where(p => !p.IsDelete);
        
        return trackChanges
            ? await query.ToListAsync()
            : await query.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<NaviProduct?> GetProductByIdAsync(int productId, bool trackChanges = false)
    {
        var query = _dbSet.Where(p => p.Id == productId && !p.IsDelete);
        
        return trackChanges
            ? await query.FirstOrDefaultAsync()
            : await query.AsNoTracking().FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<NaviProduct?> GetProductWithItemsAsync(int productId, bool trackChanges = false)
    {
        var query = _dbSet
            .Include(p => p.ProductItems!.Where(pi => !pi.IsDelete))
                .ThenInclude(pi => pi.Item)
            .Where(p => p.Id == productId && !p.IsDelete);
        
        return trackChanges
            ? await query.FirstOrDefaultAsync()
            : await query.AsNoTracking().FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<NaviProduct>> SearchProductsAsync(string searchTerm, bool trackChanges = false)
    {
        var query = _dbSet.Where(p => !p.IsDelete && 
            (p.ProductName != null && p.ProductName.Contains(searchTerm) ||
             p.Description != null && p.Description.Contains(searchTerm)));
        
        return trackChanges
            ? await query.ToListAsync()
            : await query.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<NaviProduct?> GetProductByNameAsync(string productName, bool trackChanges = false)
    {
        var query = _dbSet.Where(p => !p.IsDelete && p.ProductName == productName);

        return trackChanges
            ? await query.FirstOrDefaultAsync()
            : await query.AsNoTracking().FirstOrDefaultAsync();
    }
}
