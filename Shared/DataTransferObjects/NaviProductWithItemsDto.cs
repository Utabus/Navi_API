namespace Shared.DataTransferObjects;

/// <summary>
/// DTO cho tạo NaviProduct với Items trong một transaction
/// </summary>
public record NaviProductWithItemsForCreationDto
{
    public string? ProductName { get; init; }
    public string? Description { get; init; }
    
    /// <summary>
    /// Danh sách items mới cần tạo
    /// </summary>
    public List<NaviItemForCreationDto>? Items { get; init; }
    
    /// <summary>
    /// Danh sách IDs của items đã tồn tại cần link
    /// </summary>
    public List<int>? ExistingItemIds { get; init; }
}

/// <summary>
/// DTO cho cập nhật NaviProduct và quản lý Items
/// </summary>
public record NaviProductWithItemsForUpdateDto
{
    public string? ProductName { get; init; }
    public string? Description { get; init; }
    
    /// <summary>
    /// Danh sách items mới cần tạo và link
    /// </summary>
    public List<NaviItemForCreationDto>? ItemsToAdd { get; init; }
    
    /// <summary>
    /// Danh sách IDs của items đã tồn tại cần link thêm
    /// </summary>
    public List<int>? ItemIdsToAdd { get; init; }
    
    /// <summary>
    /// Danh sách IDs của items cần unlink (soft delete ProductItem)
    /// </summary>
    public List<int>? ItemIdsToRemove { get; init; }
}

/// <summary>
/// DTO cho NaviProduct response với danh sách Items đầy đủ
/// </summary>
public record NaviProductWithItemsDto
{
    public int Id { get; init; }
    public string? ProductName { get; init; }
    public string? Description { get; init; }
    public DateTime CDT { get; init; }
    public DateTime UDT { get; init; }
    
    /// <summary>
    /// Danh sách items liên kết với product
    /// </summary>
    public List<NaviItemDto>? Items { get; init; }
}
