using MediatR;
using Shared.DataTransferObjects;

namespace Services.Features.Navi.Commands;

// ==================== NaviItem Commands ====================

/// <summary>
/// Command tạo NaviItem mới
/// </summary>
public record CreateNaviItemCommand(NaviItemForCreationDto ItemDto) 
    : IRequest<NaviItemDto>;

/// <summary>
/// Command cập nhật NaviItem
/// </summary>
public record UpdateNaviItemCommand(int Id, NaviItemForUpdateDto ItemDto) 
    : IRequest<Unit>;

/// <summary>
/// Command xóa mềm NaviItem
/// </summary>
public record DeleteNaviItemCommand(int Id) 
    : IRequest<Unit>;

// ==================== NaviProduct Commands ====================

/// <summary>
/// Command tạo NaviProduct mới
/// </summary>
public record CreateNaviProductCommand(NaviProductForCreationDto ProductDto) 
    : IRequest<NaviProductDto>;

/// <summary>
/// Command cập nhật NaviProduct
/// </summary>
public record UpdateNaviProductCommand(int Id, NaviProductForUpdateDto ProductDto) 
    : IRequest<Unit>;

/// <summary>
/// Command xóa mềm NaviProduct
/// </summary>
public record DeleteNaviProductCommand(int Id) 
    : IRequest<Unit>;

// ==================== NaviProductItem Commands ====================

/// <summary>
/// Command tạo NaviProductItem relationship
/// </summary>
public record CreateNaviProductItemCommand(NaviProductItemForCreationDto ProductItemDto) 
    : IRequest<NaviProductItemDto>;

/// <summary>
/// Command xóa mềm NaviProductItem relationship
/// </summary>
public record DeleteNaviProductItemCommand(int Id) 
    : IRequest<Unit>;

// ==================== Transactional Commands ====================

/// <summary>
/// Command tạo Product với Items trong một transaction
/// </summary>
public record CreateProductWithItemsCommand(NaviProductWithItemsForCreationDto Dto) 
    : IRequest<NaviProductWithItemsDto>;

/// <summary>
/// Command cập nhật Product và quản lý Items trong một transaction
/// </summary>
public record UpdateProductWithItemsCommand(int ProductId, NaviProductWithItemsForUpdateDto Dto) 
    : IRequest<NaviProductWithItemsDto>;

/// <summary>
/// Command xóa mềm Product và tất cả ProductItems liên quan
/// </summary>
public record DeleteProductWithItemsCommand(int ProductId) 
    : IRequest<Unit>;

// ==================== Excel Import Command ====================

/// <summary>
/// Command import dữ liệu hàng loạt từ file Excel (.xlsx)
/// </summary>
public record ImportExcelCommand(Stream FileStream) 
    : IRequest<ExcelImportResultDto>;

// ==================== NaviHistory Commands ====================

/// <summary>
/// Command tạo NaviHistory record mới
/// </summary>
public record CreateNaviHistoryCommand(NaviHistoryForCreationDto Dto) 
    : IRequest<NaviHistoryDto>;

/// <summary>
/// Command cập nhật NaviHistory record
/// </summary>
public record UpdateNaviHistoryCommand(int Id, NaviHistoryForUpdateDto Dto) 
    : IRequest<Unit>;

/// <summary>
/// Command xóa mềm NaviHistory record
/// </summary>
public record DeleteNaviHistoryCommand(int Id) 
    : IRequest<Unit>;
