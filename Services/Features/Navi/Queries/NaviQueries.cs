using MediatR;
using Shared.DataTransferObjects;

namespace Services.Features.Navi.Queries;

// ==================== NaviItem Queries ====================

/// <summary>
/// Query lấy tất cả NaviItems
/// </summary>
public record GetAllNaviItemsQuery(bool TrackChanges = false) 
    : IRequest<IEnumerable<NaviItemDto>>;

/// <summary>
/// Query lấy NaviItem theo Id
/// </summary>
public record GetNaviItemByIdQuery(int Id, bool TrackChanges = false) 
    : IRequest<NaviItemDto?>;

/// <summary>
/// Query lấy NaviItem với Products liên quan
/// </summary>
public record GetNaviItemWithProductsQuery(int Id, bool TrackChanges = false) 
    : IRequest<NaviItemDto?>;

/// <summary>
/// Query lấy NaviItems theo Type
/// </summary>
public record GetNaviItemsByTypeQuery(string Type, bool TrackChanges = false) 
    : IRequest<IEnumerable<NaviItemDto>>;

/// <summary>
/// Query tìm kiếm NaviItems
/// </summary>
public record SearchNaviItemsQuery(string SearchTerm, bool TrackChanges = false) 
    : IRequest<IEnumerable<NaviItemDto>>;

// ==================== NaviProduct Queries ====================

/// <summary>
/// Query lấy tất cả NaviProducts
/// </summary>
public record GetAllNaviProductsQuery(bool TrackChanges = false) 
    : IRequest<IEnumerable<NaviProductDto>>;

/// <summary>
/// Query lấy NaviProduct theo Id
/// </summary>
public record GetNaviProductByIdQuery(int Id, bool TrackChanges = false) 
    : IRequest<NaviProductDto?>;

/// <summary>
/// Query lấy NaviProduct với Items liên quan
/// </summary>
public record GetNaviProductWithItemsQuery(int Id, bool TrackChanges = false) 
    : IRequest<NaviProductWithItemsDto?>;

/// <summary>
/// Query tìm kiếm NaviProducts
/// </summary>
public record SearchNaviProductsQuery(string SearchTerm, bool TrackChanges = false) 
    : IRequest<IEnumerable<NaviProductDto>>;

// ==================== NaviProductItem Queries ====================

/// <summary>
/// Query lấy tất cả NaviProductItems
/// </summary>
public record GetAllNaviProductItemsQuery(bool TrackChanges = false) 
    : IRequest<IEnumerable<NaviProductItemDto>>;

/// <summary>
/// Query lấy NaviProductItem theo Id
/// </summary>
public record GetNaviProductItemByIdQuery(int Id, bool TrackChanges = false) 
    : IRequest<NaviProductItemDto?>;

/// <summary>
/// Query lấy Items theo ProductId
/// </summary>
public record GetItemsByProductIdQuery(int ProductId, bool TrackChanges = false) 
    : IRequest<IEnumerable<NaviProductItemDto>>;

/// <summary>
/// Query lấy Products theo ItemId
/// </summary>
public record GetProductsByItemIdQuery(int ItemId, bool TrackChanges = false) 
    : IRequest<IEnumerable<NaviProductItemDto>>;

/// <summary>
/// Query kiểm tra ProductItem relationship có tồn tại không
/// </summary>
public record CheckProductItemExistsQuery(int ProductId, int ItemId) 
    : IRequest<bool>;

// ==================== NaviHistory Queries ====================

/// <summary>
/// Query lấy tất cả NaviHistories chưa bị xóa
/// </summary>
public record GetAllNaviHistoriesQuery(bool TrackChanges = false) 
    : IRequest<IEnumerable<NaviHistoryDto>>;

/// <summary>
/// Query lấy NaviHistory theo Id
/// </summary>
public record GetNaviHistoryByIdQuery(int Id, bool TrackChanges = false) 
    : IRequest<NaviHistoryDto?>;

/// <summary>
/// Query lấy NaviHistories theo mã nhân viên
/// </summary>
public record GetHistoriesByCodeNVQuery(string CodeNV, bool TrackChanges = false) 
    : IRequest<IEnumerable<NaviHistoryDto>>;

/// <summary>
/// Query lấy NaviHistories theo ProductItem Id
/// </summary>
public record GetHistoriesByProductItemIdQuery(int ProductItemId, bool TrackChanges = false) 
    : IRequest<IEnumerable<NaviHistoryDto>>;

/// <summary>
/// Query lấy NaviHistories theo Production Order
/// </summary>
public record GetHistoriesByPOQuery(string PO, bool TrackChanges = false) 
    : IRequest<IEnumerable<NaviHistoryDto>>;
