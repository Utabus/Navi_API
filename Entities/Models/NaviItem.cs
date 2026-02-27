using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

/// <summary>
/// NaviItem entity - LXA_NAVI_ITEM table
/// </summary>
[Table("LXA_NAVI_ITEM")]
public class NaviItem
{
    /// <summary>
    /// Primary key
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Mô tả
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Ghi chú
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Thông tin về bu lông
    /// </summary>
    public string? Bolts { get; set; }

    /// <summary>
    /// Thông tin về lực
    /// </summary>
    public string? Force { get; set; }

    /// <summary>
    /// Danh sách hình ảnh (JSON hoặc chuỗi phân cách)
    /// </summary>
    public string? Images { get; set; }

    /// <summary>
    /// Loại item
    /// </summary>
    public string? Type { get; set; }

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
    /// Số thứ tự bước (Step)
    /// </summary>
    public int? Step { get; set; }

    /// <summary>
    /// Navigation property - Danh sách product items liên kết
    /// </summary>
    public ICollection<NaviProductItem>? ProductItems { get; set; }
}
