using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

/// <summary>
/// NaviHistory entity - LXA_NAVI_HISTORY table
/// Lưu lịch sử thao tác của nhân viên trên từng bước sản xuất
/// </summary>
[Table("LXA_NAVI_HISTORY")]
public class NaviHistory
{
    /// <summary>
    /// Primary key
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Tên nhân viên
    /// </summary>
    public string? NameNV { get; set; }

    /// <summary>
    /// Mã nhân viên
    /// </summary>
    public string? CodeNV { get; set; }

    /// <summary>
    /// Production Order
    /// </summary>
    public string? PO { get; set; }

    /// <summary>
    /// Bước sản xuất
    /// </summary>
    public string? Step { get; set; }

    /// <summary>
    /// Foreign key đến NaviProductItem
    /// </summary>
    public int? PRODUCT_ITEM_Id { get; set; }

    /// <summary>
    /// Loại hành động
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Đánh dấu xóa mềm
    /// </summary>
    public bool IsDelete { get; set; } = false;

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
    /// Số lượng
    /// </summary>
    public int? Count { get; set; }

    /// <summary>
    /// Navigation property đến NaviProductItem
    /// </summary>
    [ForeignKey("PRODUCT_ITEM_Id")]
    public NaviProductItem? ProductItem { get; set; }
}
