using Entities.Models;

namespace Repository.Contracts;

/// <summary>
/// Interface cho NaviProductItem repository
/// </summary>
public interface INaviProductItemRepository : IRepositoryBase<NaviProductItem>
{
    /// <summary>
    /// Lấy tất cả product items chưa bị xóa
    /// </summary>
    Task<IEnumerable<NaviProductItem>> GetAllActiveProductItemsAsync(bool trackChanges = false);

    /// <summary>
    /// Lấy product item theo Id (chưa bị xóa)
    /// </summary>
    Task<NaviProductItem?> GetProductItemByIdAsync(int productItemId, bool trackChanges = false);

    /// <summary>
    /// Lấy tất cả items của một product
    /// </summary>
    Task<IEnumerable<NaviProductItem>> GetItemsByProductIdAsync(int productId, bool trackChanges = false);

    /// <summary>
    /// Lấy tất cả products của một item
    /// </summary>
    Task<IEnumerable<NaviProductItem>> GetProductsByItemIdAsync(int itemId, bool trackChanges = false);

    /// <summary>
    /// Kiểm tra xem product-item relationship có tồn tại không
    /// </summary>
    Task<bool> ProductItemExistsAsync(int productId, int itemId);

    /// <summary>
    /// Lấy product item kèm product và item details
    /// </summary>
    Task<NaviProductItem?> GetProductItemWithDetailsAsync(int productItemId, bool trackChanges = false);
}
