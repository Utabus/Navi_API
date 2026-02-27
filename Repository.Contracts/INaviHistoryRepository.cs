using Entities.Models;

namespace Repository.Contracts;

/// <summary>
/// Interface cho NaviHistory repository
/// </summary>
public interface INaviHistoryRepository : IRepositoryBase<NaviHistory>
{
    /// <summary>
    /// Lấy tất cả history records chưa bị xóa
    /// </summary>
    Task<IEnumerable<NaviHistory>> GetAllActiveHistoriesAsync(bool trackChanges = false);

    /// <summary>
    /// Lấy history theo Id (chưa bị xóa)
    /// </summary>
    Task<NaviHistory?> GetHistoryByIdAsync(int id, bool trackChanges = false);

    /// <summary>
    /// Lấy histories theo mã nhân viên
    /// </summary>
    Task<IEnumerable<NaviHistory>> GetHistoriesByCodeNVAsync(string codeNV, bool trackChanges = false);

    /// <summary>
    /// Lấy histories theo ProductItem Id
    /// </summary>
    Task<IEnumerable<NaviHistory>> GetHistoriesByProductItemIdAsync(int productItemId, bool trackChanges = false);

    /// <summary>
    /// Lấy histories theo Production Order
    /// </summary>
    Task<IEnumerable<NaviHistory>> GetHistoriesByPOAsync(string po, bool trackChanges = false);
}
