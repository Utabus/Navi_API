using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Navi.Commands;
using Services.Features.Navi.Queries;
using Shared.DataTransferObjects;
using Shared.Responses;

namespace Navi_API.Controllers;

/// <summary>
/// API Controller quản lý NaviItems
/// </summary>
[Produces("application/json")]
public class NaviItemsController : BaseApiController
{
    private readonly IMediator _mediator;

    public NaviItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy tất cả NaviItems
    /// </summary>
    /// <returns>Danh sách tất cả items</returns>
    /// <response code="200">Trả về danh sách items thành công</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllItems()
    {
        var items = await _mediator.Send(new GetAllNaviItemsQuery());
        return Success(items, "Lấy danh sách items thành công");
    }

    /// <summary>
    /// Lấy NaviItem theo Id
    /// </summary>
    /// <param name="id">Item Id</param>
    /// <returns>Item với Id tương ứng</returns>
    /// <response code="200">Trả về item thành công</response>
    /// <response code="404">Không tìm thấy item với Id này</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<NaviItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<NaviItemDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetItem(int id)
    {
        var item = await _mediator.Send(new GetNaviItemByIdQuery(id));
        
        if (item == null)
            return NotFoundResponse<NaviItemDto>($"Item với Id: {id} không tồn tại");
        
        return Success(item);
    }

    /// <summary>
    /// Lấy NaviItem với Products liên quan
    /// </summary>
    /// <param name="id">Item Id</param>
    /// <returns>Item với danh sách products</returns>
    /// <response code="200">Trả về item với products thành công</response>
    /// <response code="404">Không tìm thấy item với Id này</response>
    [HttpGet("{id:int}/products")]
    [ProducesResponseType(typeof(ApiResponse<NaviItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<NaviItemDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetItemWithProducts(int id)
    {
        var item = await _mediator.Send(new GetNaviItemWithProductsQuery(id));
        
        if (item == null)
            return NotFoundResponse<NaviItemDto>($"Item với Id: {id} không tồn tại");
        
        return Success(item);
    }

    /// <summary>
    /// Lấy NaviItems theo Type
    /// </summary>
    /// <param name="type">Type của item</param>
    /// <returns>Danh sách items có type tương ứng</returns>
    /// <response code="200">Trả về danh sách items thành công</response>
    [HttpGet("type/{type}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItemsByType(string type)
    {
        var items = await _mediator.Send(new GetNaviItemsByTypeQuery(type));
        return Success(items, $"Tìm thấy {items.Count()} items với type: {type}");
    }

    /// <summary>
    /// Tìm kiếm NaviItems
    /// </summary>
    /// <param name="term">Từ khóa tìm kiếm</param>
    /// <returns>Danh sách items phù hợp</returns>
    /// <response code="200">Trả về kết quả tìm kiếm thành công</response>
    /// <response code="400">Từ khóa tìm kiếm trống</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviItemDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviItemDto>>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchItems([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return BadRequestResponse<IEnumerable<NaviItemDto>>("Search term is required");
            
        var items = await _mediator.Send(new SearchNaviItemsQuery(term));
        return Success(items, $"Tìm thấy {items.Count()} items");
    }

    /// <summary>
    /// Tạo NaviItem mới
    /// </summary>
    /// <param name="itemDto">Thông tin item cần tạo</param>
    /// <returns>Item vừa tạo</returns>
    /// <response code="201">Tạo item thành công</response>
    /// <response code="400">Dữ liệu đầu vào không hợp lệ</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<NaviItemDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<NaviItemDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateItem([FromBody] NaviItemForCreationDto itemDto)
    {
        if (itemDto == null)
            return BadRequestResponse<NaviItemDto>("NaviItemForCreationDto object is null");

        var createdItem = await _mediator.Send(new CreateNaviItemCommand(itemDto));
        return Created(createdItem, nameof(GetItem), new { id = createdItem.Id }, "Tạo item thành công");
    }

    /// <summary>
    /// Cập nhật NaviItem
    /// </summary>
    /// <param name="id">Item Id cần cập nhật</param>
    /// <param name="itemDto">Thông tin item cần cập nhật</param>
    /// <returns>Không có nội dung</returns>
    /// <response code="200">Cập nhật item thành công</response>
    /// <response code="400">Dữ liệu đầu vào không hợp lệ</response>
    /// <response code="404">Không tìm thấy item với Id này</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem(int id, [FromBody] NaviItemForUpdateDto itemDto)
    {
        if (itemDto == null)
            return BadRequestResponse("NaviItemForUpdateDto object is null");

        await _mediator.Send(new UpdateNaviItemCommand(id, itemDto));
        return Success("Cập nhật item thành công");
    }

    /// <summary>
    /// Xóa mềm NaviItem
    /// </summary>
    /// <param name="id">Item Id cần xóa</param>
    /// <returns>Không có nội dung</returns>
    /// <response code="200">Xóa item thành công</response>
    /// <response code="404">Không tìm thấy item với Id này</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItem(int id)
    {
        await _mediator.Send(new DeleteNaviItemCommand(id));
        return Success("Xóa item thành công");
    }
}
