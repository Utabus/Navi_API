namespace Shared.DataTransferObjects;

/// <summary>
/// Kết quả trả về sau khi import file Excel
/// </summary>
public record ExcelImportResultDto
{
    /// <summary>
    /// Tổng số dòng dữ liệu trong file (không tính header)
    /// </summary>
    public int TotalRows { get; init; }

    /// <summary>
    /// Số dòng tạo mới thành công (Product + Item + ProductItem)
    /// </summary>
    public int InsertedRows { get; init; }

    /// <summary>
    /// Số dòng update item thành công (Product đã tồn tại, Item được cập nhật)
    /// </summary>
    public int UpdatedRows { get; init; }

    /// <summary>
    /// Số dòng bị bỏ qua (dữ liệu trùng lặp hoàn toàn)
    /// </summary>
    public int SkippedRows { get; init; }

    /// <summary>
    /// Số dòng thất bại do lỗi
    /// </summary>
    public int FailedRows { get; init; }

    /// <summary>
    /// Danh sách chi tiết các dòng bị lỗi
    /// </summary>
    public List<ExcelImportErrorDto> Errors { get; init; } = new();
}

/// <summary>
/// Chi tiết lỗi của từng dòng khi import Excel
/// </summary>
public record ExcelImportErrorDto
{
    /// <summary>
    /// Số thứ tự dòng trong file Excel (bắt đầu từ 2)
    /// </summary>
    public int RowNumber { get; init; }

    /// <summary>
    /// Tên sản phẩm của dòng lỗi
    /// </summary>
    public string? ProductName { get; init; }

    /// <summary>
    /// Mô tả lý do lỗi
    /// </summary>
    public string Reason { get; init; } = string.Empty;
}
