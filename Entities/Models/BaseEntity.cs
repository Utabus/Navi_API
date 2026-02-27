namespace Entities.Models;

/// <summary>
/// Base class cho tất cả entities với các properties chung
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Primary key
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Thời gian tạo record
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Thời gian cập nhật gần nhất
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
