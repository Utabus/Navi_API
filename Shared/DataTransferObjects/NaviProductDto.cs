namespace Shared.DataTransferObjects;

/// <summary>
/// DTO cho NaviProduct response
/// </summary>
public record NaviProductDto
{
    public int Id { get; init; }
    public string? ProductName { get; init; }
    public string? Description { get; init; }
    public DateTime CDT { get; init; }
    public DateTime UDT { get; init; }
}

/// <summary>
/// DTO cho tạo NaviProduct mới
/// </summary>
public record NaviProductForCreationDto
{
    public string? ProductName { get; init; }
    public string? Description { get; init; }
}

/// <summary>
/// DTO cho cập nhật NaviProduct
/// </summary>
public record NaviProductForUpdateDto
{
    public string? ProductName { get; init; }
    public string? Description { get; init; }
}
