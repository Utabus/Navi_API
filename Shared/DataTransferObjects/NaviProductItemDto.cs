namespace Shared.DataTransferObjects;

/// <summary>
/// DTO cho NaviProductItem response
/// </summary>
public record NaviProductItemDto
{
    public int Id { get; init; }
    public int? ProductId { get; init; }
    public int? ItemId { get; init; }
    public string? ProductName { get; init; }
    public string? ItemDescription { get; init; }
    public DateTime CDT { get; init; }
    public DateTime UDT { get; init; }
}

/// <summary>
/// DTO cho tạo NaviProductItem mới
/// </summary>
public record NaviProductItemForCreationDto
{
    public int ProductId { get; init; }
    public int ItemId { get; init; }
}
