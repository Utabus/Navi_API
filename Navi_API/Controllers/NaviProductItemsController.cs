using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Navi.Commands;
using Services.Features.Navi.Queries;
using Shared.DataTransferObjects;
using Shared.Responses;

namespace Navi_API.Controllers;

/// <summary>
/// API Controller quản lý NaviProductItems (relationships)
/// </summary>
[Produces("application/json")]
public class NaviProductItemsController : BaseApiController
{
    private readonly IMediator _mediator;

    public NaviProductItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy tất cả NaviProductItems
    /// </summary>
    /// <returns>Danh sách tất cả product-item relationships</returns>
    /// <response code="200">Trả về danh sách relationships thành công</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviProductItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProductItems()
    {
        var productItems = await _mediator.Send(new GetAllNaviProductItemsQuery());
        return Success(productItems, "Lấy danh sách product-item relationships thành công");
    }

    /// <summary>
    /// Lấy NaviProductItem theo Id
    /// </summary>
    /// <param name="id">ProductItem Id</param>
    /// <returns>ProductItem với Id tương ứng</returns>
    /// <response code="200">Trả về product-item relationship thành công</response>
    /// <response code="404">Không tìm thấy relationship với Id này</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<NaviProductItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<NaviProductItemDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductItem(int id)
    {
        var productItem = await _mediator.Send(new GetNaviProductItemByIdQuery(id));
        
        if (productItem == null)
            return NotFoundResponse<NaviProductItemDto>($"ProductItem với Id: {id} không tồn tại");
        
        return Success(productItem);
    }

    /// <summary>
    /// Lấy Items theo ProductId
    /// </summary>
    /// <param name="productId">Product Id</param>
    /// <returns>Danh sách items thuộc product</returns>
    /// <response code="200">Trả về danh sách items thành công</response>
    [HttpGet("product/{productId:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviProductItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItemsByProduct(int productId)
    {
        var items = await _mediator.Send(new GetItemsByProductIdQuery(productId));
        return Success(items, $"Tìm thấy {items.Count()} items cho product {productId}");
    }

    /// <summary>
    /// Lấy Products theo ItemId
    /// </summary>
    /// <param name="itemId">Item Id</param>
    /// <returns>Danh sách products chứa item</returns>
    /// <response code="200">Trả về danh sách products thành công</response>
    [HttpGet("item/{itemId:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviProductItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductsByItem(int itemId)
    {
        var products = await _mediator.Send(new GetProductsByItemIdQuery(itemId));
        return Success(products, $"Tìm thấy {products.Count()} products cho item {itemId}");
    }

    /// <summary>
    /// Kiểm tra ProductItem relationship có tồn tại không
    /// </summary>
    /// <param name="productId">Product Id</param>
    /// <param name="itemId">Item Id</param>
    /// <returns>True nếu relationship tồn tại</returns>
    /// <response code="200">Trả về kết quả kiểm tra thành công</response>
    [HttpGet("exists")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckProductItemExists([FromQuery] int productId, [FromQuery] int itemId)
    {
        var exists = await _mediator.Send(new CheckProductItemExistsQuery(productId, itemId));
        return Success(exists, exists ? "Relationship tồn tại" : "Relationship không tồn tại");
    }

    /// <summary>
    /// Tạo NaviProductItem relationship mới
    /// </summary>
    /// <param name="productItemDto">Thông tin relationship cần tạo</param>
    /// <returns>ProductItem vừa tạo</returns>
    /// <response code="201">Tạo relationship thành công</response>
    /// <response code="400">Dữ liệu đầu vào không hợp lệ hoặc product/item không tồn tại</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<NaviProductItemDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<NaviProductItemDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProductItem([FromBody] NaviProductItemForCreationDto productItemDto)
    {
        if (productItemDto == null)
            return BadRequestResponse<NaviProductItemDto>("NaviProductItemForCreationDto object is null");

        var createdProductItem = await _mediator.Send(new CreateNaviProductItemCommand(productItemDto));
        return Created(createdProductItem, nameof(GetProductItem), new { id = createdProductItem.Id }, "Tạo product-item relationship thành công");
    }

    /// <summary>
    /// Xóa mềm NaviProductItem relationship
    /// </summary>
    /// <param name="id">ProductItem Id cần xóa</param>
    /// <returns>Không có nội dung</returns>
    /// <response code="200">Xóa relationship thành công</response>
    /// <response code="404">Không tìm thấy relationship với Id này</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProductItem(int id)
    {
        await _mediator.Send(new DeleteNaviProductItemCommand(id));
        return Success("Xóa product-item relationship thành công");
    }
}
