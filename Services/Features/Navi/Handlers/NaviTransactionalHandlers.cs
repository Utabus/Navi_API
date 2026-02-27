using AutoMapper;
using Entities.Models;
using LoggerService;
using MediatR;
using Repository.Contracts;
using Services.Features.Navi.Commands;
using Shared.DataTransferObjects;

namespace Services.Features.Navi.Handlers;

// ==================== Transactional Command Handlers ====================

/// <summary>
/// Handler cho CreateProductWithItemsCommand - Tạo product + items + links trong một transaction
/// </summary>
public class CreateProductWithItemsHandler : IRequestHandler<CreateProductWithItemsCommand, NaviProductWithItemsDto>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CreateProductWithItemsHandler(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<NaviProductWithItemsDto> Handle(CreateProductWithItemsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Bắt đầu tạo product với items: {request.Dto.ProductName}");
        
        try
        {
            // Step 1: Validate existing items if provided
            if (request.Dto.ExistingItemIds?.Any() == true)
            {
                foreach (var itemId in request.Dto.ExistingItemIds)
                {
                    var item = await _repository.NaviItem.GetItemByIdAsync(itemId);
                    if (item == null)
                        throw new KeyNotFoundException($"Item với Id: {itemId} không tồn tại");
                }
            }
            
            // Step 2: Create product
            var product = _mapper.Map<NaviProduct>(request.Dto);
            product.CDT = DateTime.Now;
            product.UDT = DateTime.Now;
            _repository.NaviProduct.Create(product);
            await _repository.SaveAsync(); // Save to get product ID
            
            // Step 3: Create new items
            var createdItems = new List<NaviItem>();
            if (request.Dto.Items?.Any() == true)
            {
                foreach (var itemDto in request.Dto.Items)
                {
                    var item = _mapper.Map<NaviItem>(itemDto);
                    item.CDT = DateTime.Now;
                    item.UDT = DateTime.Now;
                    _repository.NaviItem.Create(item);
                    createdItems.Add(item);
                }
                await _repository.SaveAsync(); // Save to get item IDs
            }
            
            // Step 4: Create ProductItem relationships
            var allItemIds = createdItems.Select(i => i.Id)
                .Concat(request.Dto.ExistingItemIds ?? Enumerable.Empty<int>());
                
            foreach (var itemId in allItemIds)
            {
                var productItem = new NaviProductItem
                {
                    ProductId = product.Id,
                    ItemId = itemId,
                    CDT = DateTime.Now,
                    UDT = DateTime.Now
                };
                _repository.NaviProductItem.Create(productItem);
            }
            await _repository.SaveAsync();
            
            // Step 5: Load and return complete result
            var result = await _repository.NaviProduct.GetProductWithItemsAsync(product.Id);
            _logger.LogInfo($"Đã tạo product với {allItemIds.Count()} items");
            
            return _mapper.Map<NaviProductWithItemsDto>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Lỗi khi tạo product với items: {ex.Message}");
            throw;
        }
    }
}

/// <summary>
/// Handler cho UpdateProductWithItemsCommand - Update product và quản lý items
/// </summary>
public class UpdateProductWithItemsHandler : IRequestHandler<UpdateProductWithItemsCommand, NaviProductWithItemsDto>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public UpdateProductWithItemsHandler(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<NaviProductWithItemsDto> Handle(UpdateProductWithItemsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Bắt đầu cập nhật product với items: {request.ProductId}");
        
        try
        {
            // Step 1: Get and validate product
            var product = await _repository.NaviProduct.GetProductByIdAsync(request.ProductId, trackChanges: true);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product với Id: {request.ProductId} không tồn tại");
            }

            // Step 2: Update product info
            _mapper.Map(request.Dto, product);
            product.UDT = DateTime.Now;
            await _repository.SaveAsync();

            // Step 3: Remove items (soft delete ProductItems)
            if (request.Dto.ItemIdsToRemove?.Any() == true)
            {
                var productItemsToRemove = await _repository.NaviProductItem.GetItemsByProductIdAsync(request.ProductId, trackChanges: true);
                foreach (var productItem in productItemsToRemove.Where(pi => request.Dto.ItemIdsToRemove.Contains(pi.ItemId ?? 0)))
                {
                    productItem.IsDelete = true;
                    productItem.UDT = DateTime.Now;
                }
                await _repository.SaveAsync();
            }

            // Step 4: Create new items
            var createdItems = new List<NaviItem>();
            if (request.Dto.ItemsToAdd?.Any() == true)
            {
                foreach (var itemDto in request.Dto.ItemsToAdd)
                {
                    var item = _mapper.Map<NaviItem>(itemDto);
                    item.CDT = DateTime.Now;
                    item.UDT = DateTime.Now;
                    _repository.NaviItem.Create(item);
                    createdItems.Add(item);
                }
                await _repository.SaveAsync();
            }

            // Step 5: Validate existing items to add
            if (request.Dto.ItemIdsToAdd?.Any() == true)
            {
                foreach (var itemId in request.Dto.ItemIdsToAdd)
                {
                    var item = await _repository.NaviItem.GetItemByIdAsync(itemId);
                    if (item == null)
                        throw new KeyNotFoundException($"Item với Id: {itemId} không tồn tại");
                }
            }

            // Step 6: Link new items and existing items
            var allItemIdsToAdd = createdItems.Select(i => i.Id)
                .Concat(request.Dto.ItemIdsToAdd ?? Enumerable.Empty<int>());

            foreach (var itemId in allItemIdsToAdd)
            {
                // Check if relationship already exists
                var exists = await _repository.NaviProductItem.ProductItemExistsAsync(request.ProductId, itemId);
                if (!exists)
                {
                    var productItem = new NaviProductItem
                    {
                        ProductId = request.ProductId,
                        ItemId = itemId,
                        CDT = DateTime.Now,
                        UDT = DateTime.Now
                    };
                    _repository.NaviProductItem.Create(productItem);
                }
            }
            await _repository.SaveAsync();

            // Step 7: Load and return complete result
            var result = await _repository.NaviProduct.GetProductWithItemsAsync(request.ProductId);
            _logger.LogInfo($"Đã cập nhật product với items");
            
            return _mapper.Map<NaviProductWithItemsDto>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Lỗi khi cập nhật product với items: {ex.Message}");
            throw;
        }
    }
}

/// <summary>
/// Handler cho DeleteProductWithItemsCommand - Xóa mềm product và tất cả ProductItems
/// </summary>
public class DeleteProductWithItemsHandler : IRequestHandler<DeleteProductWithItemsCommand, Unit>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public DeleteProductWithItemsHandler(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteProductWithItemsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Bắt đầu xóa mềm product với items: {request.ProductId}");
        
        try
        {
            // Step 1: Get and validate product
            var product = await _repository.NaviProduct.GetProductByIdAsync(request.ProductId, trackChanges: true);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product với Id: {request.ProductId} không tồn tại");
            }

            // Step 2: Soft delete all ProductItems
            var productItems = await _repository.NaviProductItem.GetItemsByProductIdAsync(request.ProductId, trackChanges: true);
            foreach (var productItem in productItems)
            {
                productItem.IsDelete = true;
                productItem.UDT = DateTime.Now;
            }

            // Step 3: Soft delete product
            product.IsDelete = true;
            product.UDT = DateTime.Now;

            await _repository.SaveAsync();

            _logger.LogInfo($"Đã xóa mềm product và {productItems.Count()} ProductItems");
            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Lỗi khi xóa mềm product với items: {ex.Message}");
            throw;
        }
    }
}
