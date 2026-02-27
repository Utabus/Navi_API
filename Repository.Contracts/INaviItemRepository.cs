using Entities.Models;

namespace Repository.Contracts;

/// <summary>
/// Interface cho NaviItem repository
/// </summary>
public interface INaviItemRepository : IRepositoryBase<NaviItem>
{
    /// <summary>
    /// Lấy tất cả items chưa bị xóa
    /// </summary>
    Task<IEnumerable<NaviItem>> GetAllActiveItemsAsync(bool trackChanges = false);

    /// <summary>
    /// Lấy item theo Id (chưa bị xóa)
    /// </summary>
    Task<NaviItem?> GetItemByIdAsync(int itemId, bool trackChanges = false);

    /// <summary>
    /// Lấy item kèm danh sách product items
    /// </summary>
    Task<NaviItem?> GetItemWithProductItemsAsync(int itemId, bool trackChanges = false);

    /// <summary>
    /// Lấy items theo loại (Type)
    /// </summary>
    Task<IEnumerable<NaviItem>> GetItemsByTypeAsync(string type, bool trackChanges = false);

    /// <summary>
    /// Tìm kiếm items theo description hoặc note
    /// </summary>
    Task<IEnumerable<NaviItem>> SearchItemsAsync(string searchTerm, bool trackChanges = false);
}
