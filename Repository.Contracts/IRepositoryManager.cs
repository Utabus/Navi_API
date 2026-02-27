namespace Repository.Contracts;

/// <summary>
/// Unit of Work pattern - quản lý tất cả repositories
/// </summary>
public interface IRepositoryManager
{

    INaviItemRepository NaviItem { get; }

    /// <summary>
    /// NaviProduct repository
    /// </summary>
    INaviProductRepository NaviProduct { get; }

    /// <summary>
    /// NaviProductItem repository
    /// </summary>
    INaviProductItemRepository NaviProductItem { get; }

    /// <summary>
    /// NaviHistory repository
    /// </summary>
    INaviHistoryRepository NaviHistory { get; }

    /// <summary>
    /// Lưu tất cả thay đổi vào database
    /// </summary>
    Task SaveAsync();
}
