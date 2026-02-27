using Entities;
using Repository.Contracts;
using Repository.EfCore;

namespace Repository;

/// <summary>
/// Unit of Work implementation - quản lý tất cả repositories
/// Sử dụng lazy loading để tối ưu performance
/// </summary>
public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _context;
    private readonly Lazy<INaviItemRepository> _naviItemRepository;
    private readonly Lazy<INaviProductRepository> _naviProductRepository;
    private readonly Lazy<INaviProductItemRepository> _naviProductItemRepository;
    private readonly Lazy<INaviHistoryRepository> _naviHistoryRepository;

    public RepositoryManager(RepositoryContext context)
    {
        _context = context;
        _naviItemRepository = new Lazy<INaviItemRepository>(() => new NaviItemRepository(context));
        _naviProductRepository = new Lazy<INaviProductRepository>(() => new NaviProductRepository(context));
        _naviProductItemRepository = new Lazy<INaviProductItemRepository>(() => new NaviProductItemRepository(context));
        _naviHistoryRepository = new Lazy<INaviHistoryRepository>(() => new NaviHistoryRepository(context));
    }

    /// <inheritdoc/>

    /// <inheritdoc/>
    public INaviItemRepository NaviItem => _naviItemRepository.Value;

    /// <inheritdoc/>
    public INaviProductRepository NaviProduct => _naviProductRepository.Value;

    /// <inheritdoc/>
    public INaviProductItemRepository NaviProductItem => _naviProductItemRepository.Value;

    /// <inheritdoc/>
    public INaviHistoryRepository NaviHistory => _naviHistoryRepository.Value;

    /// <inheritdoc/>
    public async Task SaveAsync() => await _context.SaveChangesAsync();
}
