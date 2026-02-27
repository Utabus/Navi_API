using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Navi.Commands;
using Services.Features.Navi.Queries;
using Shared.DataTransferObjects;
using Shared.Responses;

namespace Navi_API.Controllers;

/// <summary>
/// API Controller quản lý NaviHistory (lịch sử thao tác NV)
/// </summary>
[Produces("application/json")]
public class NaviHistoryController : BaseApiController
{
    private readonly IMediator _mediator;

    public NaviHistoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy tất cả NaviHistory records chưa bị xóa
    /// </summary>
    /// <returns>Danh sách tất cả history records</returns>
    /// <response code="200">Trả về danh sách histories thành công</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviHistoryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllHistories()
    {
        var histories = await _mediator.Send(new GetAllNaviHistoriesQuery());
        return Success(histories, "Lấy danh sách lịch sử thành công");
    }

    /// <summary>
    /// Lấy NaviHistory theo Id
    /// </summary>
    /// <param name="id">History Id</param>
    /// <returns>History record với Id tương ứng</returns>
    /// <response code="200">Trả về history thành công</response>
    /// <response code="404">Không tìm thấy history với Id này</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<NaviHistoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<NaviHistoryDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHistory(int id)
    {
        var history = await _mediator.Send(new GetNaviHistoryByIdQuery(id));

        if (history == null)
            return NotFoundResponse<NaviHistoryDto>($"NaviHistory với Id: {id} không tồn tại");

        return Success(history);
    }

    /// <summary>
    /// Lấy NaviHistories theo mã nhân viên
    /// </summary>
    /// <param name="codeNV">Mã nhân viên</param>
    /// <returns>Danh sách histories của nhân viên</returns>
    /// <response code="200">Trả về danh sách histories thành công</response>
    [HttpGet("nv/{codeNV}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviHistoryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistoriesByNV(string codeNV)
    {
        var histories = await _mediator.Send(new GetHistoriesByCodeNVQuery(codeNV));
        return Success(histories, $"Tìm thấy {histories.Count()} records cho nhân viên {codeNV}");
    }

    /// <summary>
    /// Lấy NaviHistories theo ProductItem Id
    /// </summary>
    /// <param name="productItemId">ProductItem Id</param>
    /// <returns>Danh sách histories của ProductItem</returns>
    /// <response code="200">Trả về danh sách histories thành công</response>
    [HttpGet("productitem/{productItemId:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviHistoryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistoriesByProductItem(int productItemId)
    {
        var histories = await _mediator.Send(new GetHistoriesByProductItemIdQuery(productItemId));
        return Success(histories, $"Tìm thấy {histories.Count()} records cho ProductItem {productItemId}");
    }

    /// <summary>
    /// Lấy NaviHistories theo Production Order
    /// </summary>
    /// <param name="po">Mã Production Order</param>
    /// <returns>Danh sách histories của PO</returns>
    /// <response code="200">Trả về danh sách histories thành công</response>
    [HttpGet("po/{po}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviHistoryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistoriesByPO(string po)
    {
        var histories = await _mediator.Send(new GetHistoriesByPOQuery(po));
        return Success(histories, $"Tìm thấy {histories.Count()} records cho PO {po}");
    }

    /// <summary>
    /// Tạo NaviHistory record mới
    /// </summary>
    /// <param name="historyDto">Thông tin history cần tạo</param>
    /// <returns>History vừa tạo</returns>
    /// <response code="201">Tạo history thành công</response>
    /// <response code="400">Dữ liệu đầu vào không hợp lệ</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<NaviHistoryDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<NaviHistoryDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHistory([FromBody] NaviHistoryForCreationDto historyDto)
    {
        if (historyDto == null)
            return BadRequestResponse<NaviHistoryDto>("NaviHistoryForCreationDto object is null");

        var createdHistory = await _mediator.Send(new CreateNaviHistoryCommand(historyDto));
        return Created(createdHistory, nameof(GetHistory), new { id = createdHistory.Id }, "Tạo history thành công");
    }

    /// <summary>
    /// Cập nhật NaviHistory record
    /// </summary>
    /// <param name="id">History Id cần cập nhật</param>
    /// <param name="historyDto">Thông tin cập nhật</param>
    /// <returns>Không có nội dung</returns>
    /// <response code="200">Cập nhật history thành công</response>
    /// <response code="400">Dữ liệu đầu vào không hợp lệ</response>
    /// <response code="404">Không tìm thấy history với Id này</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHistory(int id, [FromBody] NaviHistoryForUpdateDto historyDto)
    {
        if (historyDto == null)
            return BadRequestResponse("NaviHistoryForUpdateDto object is null");

        await _mediator.Send(new UpdateNaviHistoryCommand(id, historyDto));
        return Success("Cập nhật history thành công");
    }

    /// <summary>
    /// Xóa mềm NaviHistory record
    /// </summary>
    /// <param name="id">History Id cần xóa</param>
    /// <returns>Không có nội dung</returns>
    /// <response code="200">Xóa history thành công</response>
    /// <response code="404">Không tìm thấy history với Id này</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteHistory(int id)
    {
        await _mediator.Send(new DeleteNaviHistoryCommand(id));
        return Success("Xóa history thành công");
    }
}
