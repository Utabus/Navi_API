namespace Shared.DataTransferObjects;

/// <summary>
/// DTO cho NaviItem response
/// </summary>
public record NaviItemDto
{
    public int Id { get; init; }
    public string? Description { get; init; }
    public string? Note { get; init; }
    public string? Bolts { get; init; }
    public string? Force { get; init; }
    public string? Images { get; init; }
    public string? Type { get; init; }
    public int? Step { get; init; }
    public DateTime CDT { get; init; }
    public DateTime UDT { get; init; }
}

/// <summary>
/// DTO cho tạo NaviItem mới
/// </summary>
public record NaviItemForCreationDto
{
    public string? Description { get; init; }
    public string? Note { get; init; }
    public string? Bolts { get; init; }
    public string? Force { get; init; }
    public string? Images { get; init; }
    public string? Type { get; init; }
    public int? Step { get; init; }
}

/// <summary>
/// DTO cho cập nhật NaviItem
/// </summary>
public record NaviItemForUpdateDto
{
    public string? Description { get; init; }
    public string? Note { get; init; }
    public string? Bolts { get; init; }
    public string? Force { get; init; }
    public string? Images { get; init; }
    public string? Type { get; init; }
    public int? Step { get; init; }
}
