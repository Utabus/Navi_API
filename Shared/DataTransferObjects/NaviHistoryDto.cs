namespace Shared.DataTransferObjects;

/// <summary>
/// DTO cho NaviHistory response
/// </summary>
public record NaviHistoryDto
{
    public int Id { get; init; }
    public string? NameNV { get; init; }
    public string? CodeNV { get; init; }
    public string? PO { get; init; }
    public string? Step { get; init; }
    public int? PRODUCT_ITEM_Id { get; init; }
    public string? Type { get; init; }
    public bool IsDelete { get; init; }
    public DateTime CDT { get; init; }
    public DateTime UDT { get; init; }
    public int? Count { get; init; }
}

/// <summary>
/// DTO cho tạo NaviHistory mới (POST)
/// </summary>
public record NaviHistoryForCreationDto
{
    public string? NameNV { get; init; }
    public string? CodeNV { get; init; }
    public string? PO { get; init; }
    public string? Step { get; init; }
    public int? PRODUCT_ITEM_Id { get; init; }
    public string? Type { get; init; }
    public int? Count { get; init; }
}

/// <summary>
/// DTO cho cập nhật NaviHistory (PUT)
/// </summary>
public record NaviHistoryForUpdateDto
{
    public string? NameNV { get; init; }
    public string? CodeNV { get; init; }
    public string? PO { get; init; }
    public string? Step { get; init; }
    public int? PRODUCT_ITEM_Id { get; init; }
    public string? Type { get; init; }
    public int? Count { get; init; }
}
