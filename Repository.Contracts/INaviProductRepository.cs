using Entities.Models;

namespace Repository.Contracts;

/// <summary>
/// Interface cho NaviProduct repository
/// </summary>
public interface INaviProductRepository : IRepositoryBase<NaviProduct>
{
    /// <summary>
    /// Lấy tất cả products chưa bị xóa
    /// </summary>
    Task<IEnumerable<NaviProduct>> GetAllActiveProductsAsync(bool trackChanges = false);

    /// <summary>
    /// Lấy product theo Id (chưa bị xóa)
    /// </summary>
    Task<NaviProduct?> GetProductByIdAsync(int productId, bool trackChanges = false);

    /// <summary>
    /// Lấy product kèm danh sách items
    /// </summary>
    Task<NaviProduct?> GetProductWithItemsAsync(int productId, bool trackChanges = false);

    /// <summary>
    /// Tìm kiếm products theo tên hoặc mô tả
    /// </summary>
    Task<IEnumerable<NaviProduct>> SearchProductsAsync(string searchTerm, bool trackChanges = false);

    /// <summary>
    /// Lấy product theo tên chính xác (dùng cho import Excel)
    /// </summary>
    Task<NaviProduct?> GetProductByNameAsync(string productName, bool trackChanges = false);
}
