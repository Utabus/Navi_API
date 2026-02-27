using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

/// <summary>
/// NaviProductItem entity - LXA_NAVI_PRODUCT_ITEM table (Junction/Bridge table)
/// </summary>
[Table("LXA_NAVI_PRODUCT_ITEM")]
public class NaviProductItem
{
    /// <summary>
    /// Primary key
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Foreign key đến NaviProduct
    /// </summary>
    public int? ProductId { get; set; }

    /// <summary>
    /// Foreign key đến NaviItem
    /// </summary>
    public int? ItemId { get; set; }

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
    /// Navigation property đến NaviProduct
    /// </summary>
    [ForeignKey("ProductId")]
    public NaviProduct? Product { get; set; }

    /// <summary>
    /// Navigation property đến NaviItem
    /// </summary>
    [ForeignKey("ItemId")]
    public NaviItem? Item { get; set; }
}
