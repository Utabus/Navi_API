using ClosedXML.Excel;
using Entities.Models;
using LoggerService;
using MediatR;
using Repository.Contracts;
using Services.Features.Navi.Commands;
using Shared.DataTransferObjects;

namespace Services.Features.Navi.Handlers;

/// <summary>
/// Handler import dữ liệu từ file Excel (.xlsx) vào 3 bảng:
/// LXA_NAVI_PRODUCT, LXA_NAVI_ITEM, LXA_NAVI_PRODUCT_ITEM
///
/// Cấu trúc file Excel:
/// - Mỗi SHEET = 1 NaviProduct (ProductName = tên sheet)
/// - Row 1: Header (#, Description, Bolts, Force, Images)
/// - Row 2+: Dữ liệu NaviItem
///   Cột A = Step (#)
///   Cột B = Description
///   Cột C = Bolts
///   Cột D = Force
///   Cột E = Images
///
/// Logic upsert:
/// - Product chưa có → tạo mới
/// - Product đã có + Item có cùng Step → UPDATE Item đó
/// - Product đã có + chưa có Item cùng Step → tạo NaviItem + NaviProductItem mới
/// </summary>
public class ImportExcelHandler : IRequestHandler<ImportExcelCommand, ExcelImportResultDto>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public ImportExcelHandler(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ExcelImportResultDto> Handle(ImportExcelCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo("Bắt đầu import dữ liệu từ file Excel...");

        var errors      = new List<ExcelImportErrorDto>();
        int insertedRows = 0;
        int updatedRows  = 0;
        int skippedRows  = 0;
        int failedRows   = 0;
        int totalRows    = 0;

        using var workbook = new XLWorkbook(request.FileStream);

        // --- Duyệt từng sheet (mỗi sheet = 1 NaviProduct) ---
        foreach (var worksheet in workbook.Worksheets)
        {
            string productName = worksheet.Name.Trim();
            _logger.LogInfo($"Đang xử lý sheet: '{productName}'");

            // Tìm hoặc tạo NaviProduct
            var product = await _repository.NaviProduct.GetProductByNameAsync(productName, trackChanges: true);
            bool isNewProduct = product == null;

            if (isNewProduct)
            {
                product = new NaviProduct
                {
                    ProductName = productName,
                    Description = null,   // ProductDescription luôn null
                    CDT         = DateTime.Now,
                    UDT         = DateTime.Now,
                    IsDelete    = false
                };
                _repository.NaviProduct.Create(product);
                await _repository.SaveAsync();
                _logger.LogInfo($"  → Tạo NaviProduct mới '{productName}' (Id={product.Id})");
            }

            // Load Product kèm Items đã liên kết (để match Step khi upsert)
            // Key = Step, Value = NaviItem (trackChanges=true để có thể update)
            var existingItemsByStep = await BuildExistingItemsMapAsync(product!.Id);

            int lastDataRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;

            // --- Duyệt từng dòng dữ liệu (bỏ header row 1) ---
            for (int rowNum = 2; rowNum <= lastDataRow; rowNum++)
            {
                var row = worksheet.Row(rowNum);
                if (row.IsEmpty()) { skippedRows++; continue; }

                totalRows++;

                // Parse cột theo vị trí cố định
                int?    step        = ParseInt(row.Cell(1));      // Cột A: #
                string? description = ParseString(row.Cell(2));   // Cột B: Description
                string? bolts       = ParseString(row.Cell(3));   // Cột C: Bolts
                string? force       = ParseString(row.Cell(4));   // Cột D: Force
                string? images      = ParseString(row.Cell(5));   // Cột E: Images

                try
                {
                    if (!isNewProduct && step.HasValue && existingItemsByStep.TryGetValue(step.Value, out var existingItem))
                    {
                        // --- UPDATE: Product đã có, tìm thấy Item cùng Step ---
                        existingItem.Description = description;
                        existingItem.Bolts       = bolts;
                        existingItem.Force       = force;
                        existingItem.Images      = images;
                        existingItem.Step        = step;
                        existingItem.UDT         = DateTime.Now;
                        await _repository.SaveAsync();

                        updatedRows++;
                        _logger.LogInfo($"  Row {rowNum}: UPDATE NaviItem Id={existingItem.Id} (Step={step})");
                    }
                    else
                    {
                        // --- INSERT: Product mới || không tìm thấy Item cùng Step ---
                        var newItem = new NaviItem
                        {
                            Description = description,
                            Note        = null,   // Không có trong file
                            Bolts       = bolts,
                            Force       = force,
                            Images      = images,
                            Type        = null,   // Không có trong file
                            Step        = step,
                            CDT         = DateTime.Now,
                            UDT         = DateTime.Now,
                            IsDelete    = false
                        };
                        _repository.NaviItem.Create(newItem);
                        await _repository.SaveAsync();

                        var productItem = new NaviProductItem
                        {
                            ProductId = product!.Id,
                            ItemId    = newItem.Id,
                            CDT       = DateTime.Now,
                            UDT       = DateTime.Now,
                            IsDelete  = false
                        };
                        _repository.NaviProductItem.Create(productItem);
                        await _repository.SaveAsync();

                        // Cập nhật map để tránh duplicate trong cùng file
                        if (step.HasValue)
                            existingItemsByStep[step.Value] = newItem;

                        insertedRows++;
                        _logger.LogInfo($"  Row {rowNum}: INSERT NaviItem Id={newItem.Id} (Step={step})");
                    }
                }
                catch (Exception ex)
                {
                    failedRows++;
                    errors.Add(new ExcelImportErrorDto
                    {
                        RowNumber   = rowNum,
                        ProductName = productName,
                        Reason      = ex.Message
                    });
                    _logger.LogError($"  Row {rowNum} lỗi: {ex.Message}");
                }
            }
        }

        _logger.LogInfo($"Import hoàn tất — Inserted:{insertedRows} Updated:{updatedRows} Skipped:{skippedRows} Failed:{failedRows}");

        return new ExcelImportResultDto
        {
            TotalRows    = totalRows,
            InsertedRows = insertedRows,
            UpdatedRows  = updatedRows,
            SkippedRows  = skippedRows,
            FailedRows   = failedRows,
            Errors       = errors
        };
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Load tất cả NaviItems đã liên kết với Product, build map Step → NaviItem.
    /// trackChanges=true để có thể update trực tiếp.
    /// </summary>
    private async Task<Dictionary<int, NaviItem>> BuildExistingItemsMapAsync(int productId)
    {
        var map         = new Dictionary<int, NaviItem>();
        var productData = await _repository.NaviProduct.GetProductWithItemsAsync(productId, trackChanges: true);
        if (productData?.ProductItems == null) return map;

        foreach (var pi in productData.ProductItems)
        {
            if (pi.Item != null && pi.Item.Step.HasValue && !pi.IsDelete && !pi.Item.IsDelete)
                map[pi.Item.Step.Value] = pi.Item;
        }
        return map;
    }

    /// <summary>Đọc giá trị string của cell, trả về null nếu rỗng</summary>
    private static string? ParseString(IXLCell cell)
    {
        if (cell.IsEmpty()) return null;
        var val = cell.GetValue<string>()?.Trim();
        return string.IsNullOrWhiteSpace(val) ? null : val;
    }

    /// <summary>Đọc giá trị int? của cell</summary>
    private static int? ParseInt(IXLCell cell)
    {
        if (cell.IsEmpty()) return null;
        return int.TryParse(cell.GetValue<string>(), out int result) ? result : null;
    }
}

