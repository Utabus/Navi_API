using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Navi.Commands;
using Services.Features.Navi.Queries;
using Shared.DataTransferObjects;
using Shared.Responses;

namespace Navi_API.Controllers;

/// <summary>
/// API Controller quản lý NaviProducts với transactional operations
/// </summary>
[Produces("application/json")]
public class NaviProductsController : BaseApiController
{
    private readonly IMediator _mediator;

    public NaviProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy tất cả NaviProducts
    /// </summary>
    /// <returns>Danh sách tất cả products</returns>
    /// <response code="200">Trả về danh sách products thành công</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviProductDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _mediator.Send(new GetAllNaviProductsQuery());
        return Success(products, "Lấy danh sách products thành công");
    }

    /// <summary>
    /// Lấy NaviProduct theo Id
    /// </summary>
    /// <param name="id">Product Id</param>
    /// <returns>Product với Id tương ứng</returns>
    /// <response code="200">Trả về product thành công</response>
    /// <response code="404">Không tìm thấy product với Id này</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<NaviProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<NaviProductDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _mediator.Send(new GetNaviProductByIdQuery(id));
        
        if (product == null)
            return NotFoundResponse<NaviProductDto>($"Product với Id: {id} không tồn tại");
        
        return Success(product);
    }

    /// <summary>
    /// Lấy NaviProduct với Items liên quan
    /// </summary>
    /// <param name="id">Product Id</param>
    /// <returns>Product với danh sách items</returns>
    /// <response code="200">Trả về product với items thành công</response>
    /// <response code="404">Không tìm thấy product với Id này</response>
    [HttpGet("{id:int}/items")]
    [ProducesResponseType(typeof(ApiResponse<NaviProductWithItemsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<NaviProductWithItemsDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductWithItems(int id)
    {
        var product = await _mediator.Send(new GetNaviProductWithItemsQuery(id));
        
        if (product == null)
            return NotFoundResponse<NaviProductWithItemsDto>($"Product với Id: {id} không tồn tại");
        
        return Success(product);
    }

    /// <summary>
    /// Tìm kiếm NaviProducts
    /// </summary>
    /// <param name="term">Từ khóa tìm kiếm</param>
    /// <returns>Danh sách products phù hợp</returns>
    /// <response code="200">Trả về kết quả tìm kiếm thành công</response>
    /// <response code="400">Từ khóa tìm kiếm trống</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviProductDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NaviProductDto>>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchProducts([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return BadRequestResponse<IEnumerable<NaviProductDto>>("Search term is required");
            
        var products = await _mediator.Send(new SearchNaviProductsQuery(term));
        return Success(products, $"Tìm thấy {products.Count()} products");
    }

    /// <summary>
    /// Tạo NaviProduct mới
    /// </summary>
    /// <param name="productDto">Thông tin product cần tạo</param>
    /// <returns>Product vừa tạo</returns>
    /// <response code="201">Tạo product thành công</response>
    /// <response code="400">Dữ liệu đầu vào không hợp lệ</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<NaviProductDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<NaviProductDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] NaviProductForCreationDto productDto)
    {
        if (productDto == null)
            return BadRequestResponse<NaviProductDto>("NaviProductForCreationDto object is null");

        var createdProduct = await _mediator.Send(new CreateNaviProductCommand(productDto));
        return Created(createdProduct, nameof(GetProduct), new { id = createdProduct.Id }, "Tạo product thành công");
    }

    /// <summary>
    /// Cập nhật NaviProduct
    /// </summary>
    /// <param name="id">Product Id cần cập nhật</param>
    /// <param name="productDto">Thông tin product cần cập nhật</param>
    /// <returns>Không có nội dung</returns>
    /// <response code="200">Cập nhật product thành công</response>
    /// <response code="400">Dữ liệu đầu vào không hợp lệ</response>
    /// <response code="404">Không tìm thấy product với Id này</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] NaviProductForUpdateDto productDto)
    {
        if (productDto == null)
            return BadRequestResponse("NaviProductForUpdateDto object is null");

        await _mediator.Send(new UpdateNaviProductCommand(id, productDto));
        return Success("Cập nhật product thành công");
    }

    /// <summary>
    /// Xóa mềm NaviProduct
    /// </summary>
    /// <param name="id">Product Id cần xóa</param>
    /// <returns>Không có nội dung</returns>
    /// <response code="200">Xóa product thành công</response>
    /// <response code="404">Không tìm thấy product với Id này</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _mediator.Send(new DeleteNaviProductCommand(id));
        return Success("Xóa product thành công");
    }

    // ==================== Transactional Endpoints ====================

    /// <summary>
    /// Tạo Product với Items trong một transaction
    /// </summary>
    /// <param name="dto">Thông tin product và items cần tạo</param>
    /// <returns>Product với items vừa tạo</returns>
    /// <response code="201">Tạo product với items thành công</response>
    /// <response code="400">Dữ liệu đầu vào không hợp lệ hoặc item không tồn tại</response>
    [HttpPost("with-items")]
    [ProducesResponseType(typeof(ApiResponse<NaviProductWithItemsDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<NaviProductWithItemsDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProductWithItems([FromBody] NaviProductWithItemsForCreationDto dto)
    {
        if (dto == null)
            return BadRequestResponse<NaviProductWithItemsDto>("NaviProductWithItemsForCreationDto object is null");

        var result = await _mediator.Send(new CreateProductWithItemsCommand(dto));
        return Created(result, nameof(GetProductWithItems), new { id = result.Id }, "Tạo product với items thành công");
    }

    /// <summary>
    /// Cập nhật Product và quản lý Items trong một transaction
    /// </summary>
    /// <param name="id">Product Id cần cập nhật</param>
    /// <param name="dto">Thông tin cập nhật product và items</param>
    /// <returns>Product với items sau khi cập nhật</returns>
    /// <response code="200">Cập nhật product với items thành công</response>
    /// <response code="400">Dữ liệu đầu vào không hợp lệ</response>
    /// <response code="404">Không tìm thấy product với Id này</response>
    [HttpPut("{id:int}/with-items")]
    [ProducesResponseType(typeof(ApiResponse<NaviProductWithItemsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<NaviProductWithItemsDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<NaviProductWithItemsDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProductWithItems(int id, [FromBody] NaviProductWithItemsForUpdateDto dto)
    {
        if (dto == null)
            return BadRequestResponse<NaviProductWithItemsDto>("NaviProductWithItemsForUpdateDto object is null");

        var result = await _mediator.Send(new UpdateProductWithItemsCommand(id, dto));
        return Success(result, "Cập nhật product với items thành công");
    }

    /// <summary>
    /// Xóa mềm Product và tất cả ProductItems liên quan
    /// </summary>
    /// <param name="id">Product Id cần xóa</param>
    /// <returns>Không có nội dung</returns>
    /// <response code="200">Xóa product với items thành công</response>
    /// <response code="404">Không tìm thấy product với Id này</response>
    [HttpDelete("{id:int}/with-items")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProductWithItems(int id)
    {
        await _mediator.Send(new DeleteProductWithItemsCommand(id));
        return Success("Xóa product với items thành công");
    }

    // ==================== Excel Import Endpoint ====================

    /// <summary>
    /// Import hàng loạt dữ liệu từ file Excel (.xlsx) vào NaviProduct, NaviItem, NaviProductItem
    /// </summary>
    /// <remarks>
    /// File Excel cần có header row với các cột:
    /// ProductName, ProductDescription, ItemDescription, ItemNote, ItemBolts, ItemForce, ItemType, Step
    /// 
    /// Logic xử lý:
    /// - ProductName chưa có → tạo Product + Item + ProductItem mới
    /// - ProductName đã có + đã có Item liên kết → UPDATE Item đó
    /// - ProductName đã có + chưa có Item liên kết → tạo Item + ProductItem mới
    /// </remarks>
    /// <param name="file">File Excel (.xlsx), tối đa 10MB</param>
    /// <returns>Kết quả import: số dòng inserted/updated/skipped/failed</returns>
    /// <response code="200">Import hoàn tất (có thể có một số dòng lỗi — xem Errors)</response>
    /// <response code="400">File không hợp lệ (null, không phải .xlsx, vượt 10MB)</response>
    [HttpPost("import-excel")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponse<ExcelImportResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportFromExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequestResponse<ExcelImportResultDto>("File không được để trống");

        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            return BadRequestResponse<ExcelImportResultDto>("Chỉ chấp nhận file định dạng .xlsx");

        if (file.Length > 10 * 1024 * 1024)
            return BadRequestResponse<ExcelImportResultDto>("File không được vượt quá 10MB");

        using var stream = file.OpenReadStream();
        var result = await _mediator.Send(new ImportExcelCommand(stream));

        var message = $"Import hoàn tất: {result.InsertedRows} inserted, {result.UpdatedRows} updated, " +
                      $"{result.SkippedRows} skipped, {result.FailedRows} failed / tổng {result.TotalRows} dòng";

        return Success(result, message);
    }
}
