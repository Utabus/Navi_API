using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

/// <summary>
/// NaviProduct entity - LXA_NAVI_PRODUCT table
/// </summary>
[Table("LXA_NAVI_PRODUCT")]
public class NaviProduct
{
    /// <summary>
    /// Primary key
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Tên sản phẩm
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// Mô tả sản phẩm
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Thời gian tạo (Create DateTime)
    /// </summary>
    [Column("CDT")]
    public DateTime CDT { get; set; } = DateTime.Now;

    /// <summary>
    /// Thời gian cập nhật (Update DateTime)
    /// </summary>
    [Column("UDT")]
    public DateTime UDT { get; set; } = DateTime.Now;

    /// <summary>
    /// Đánh dấu xóa (soft delete)
    /// </summary>
    public bool IsDelete { get; set; } = false;

    /// <summary>
    /// Navigation property - Danh sách product items liên kết
    /// </summary>
    public ICollection<NaviProductItem>? ProductItems { get; set; }
}
