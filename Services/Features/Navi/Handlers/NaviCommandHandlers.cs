using AutoMapper;
using Entities.Models;
using LoggerService;
using MediatR;
using Repository.Contracts;
using Services.Features.Navi.Commands;
using Shared.DataTransferObjects;

namespace Services.Features.Navi.Handlers;

// ==================== NaviItem Command Handlers ====================

/// <summary>
/// Handler cho CreateNaviItemCommand
/// </summary>
public class CreateNaviItemHandler : IRequestHandler<CreateNaviItemCommand, NaviItemDto>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CreateNaviItemHandler(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<NaviItemDto> Handle(CreateNaviItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Đang tạo NaviItem mới: {request.ItemDto.Description}");

        var item = _mapper.Map<NaviItem>(request.ItemDto);
        item.CDT = DateTime.Now;
        item.UDT = DateTime.Now;

        _repository.NaviItem.Create(item);
        await _repository.SaveAsync();

        _logger.LogInfo($"Đã tạo NaviItem với Id: {item.Id}");
        return _mapper.Map<NaviItemDto>(item);
    }
}

/// <summary>
/// Handler cho UpdateNaviItemCommand
/// </summary>
public class UpdateNaviItemHandler : IRequestHandler<UpdateNaviItemCommand, Unit>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public UpdateNaviItemHandler(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateNaviItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Đang cập nhật NaviItem: {request.Id}");

        var item = await _repository.NaviItem.GetItemByIdAsync(request.Id, trackChanges: true);
        if (item == null)
        {
            throw new KeyNotFoundException($"NaviItem với Id: {request.Id} không tồn tại");
        }

        _mapper.Map(request.ItemDto, item);
        item.UDT = DateTime.Now;
        await _repository.SaveAsync();

        _logger.LogInfo($"Đã cập nhật NaviItem: {request.Id}");
        return Unit.Value;
    }
}

/// <summary>
/// Handler cho DeleteNaviItemCommand
/// </summary>
public class DeleteNaviItemHandler : IRequestHandler<DeleteNaviItemCommand, Unit>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public DeleteNaviItemHandler(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteNaviItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Đang xóa mềm NaviItem: {request.Id}");

        var item = await _repository.NaviItem.GetItemByIdAsync(request.Id, trackChanges: true);
        if (item == null)
        {
            throw new KeyNotFoundException($"NaviItem với Id: {request.Id} không tồn tại");
        }

        item.IsDelete = true;
        item.UDT = DateTime.Now;
        await _repository.SaveAsync();

        _logger.LogInfo($"Đã xóa mềm NaviItem: {request.Id}");
        return Unit.Value;
    }
}

// ==================== NaviProduct Command Handlers ====================

/// <summary>
/// Handler cho CreateNaviProductCommand
/// </summary>
public class CreateNaviProductHandler : IRequestHandler<CreateNaviProductCommand, NaviProductDto>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CreateNaviProductHandler(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<NaviProductDto> Handle(CreateNaviProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Đang tạo NaviProduct mới: {request.ProductDto.ProductName}");

        var product = _mapper.Map<NaviProduct>(request.ProductDto);
        product.CDT = DateTime.Now;
        product.UDT = DateTime.Now;

        _repository.NaviProduct.Create(product);
        await _repository.SaveAsync();

        _logger.LogInfo($"Đã tạo NaviProduct với Id: {product.Id}");
        return _mapper.Map<NaviProductDto>(product);
    }
}

/// <summary>
/// Handler cho UpdateNaviProductCommand
/// </summary>
public class UpdateNaviProductHandler : IRequestHandler<UpdateNaviProductCommand, Unit>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public UpdateNaviProductHandler(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateNaviProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Đang cập nhật NaviProduct: {request.Id}");

        var product = await _repository.NaviProduct.GetProductByIdAsync(request.Id, trackChanges: true);
        if (product == null)
        {
            throw new KeyNotFoundException($"NaviProduct với Id: {request.Id} không tồn tại");
        }

        _mapper.Map(request.ProductDto, product);
        product.UDT = DateTime.Now;
        await _repository.SaveAsync();

        _logger.LogInfo($"Đã cập nhật NaviProduct: {request.Id}");
        return Unit.Value;
    }
}

/// <summary>
/// Handler cho DeleteNaviProductCommand
/// </summary>
public class DeleteNaviProductHandler : IRequestHandler<DeleteNaviProductCommand, Unit>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public DeleteNaviProductHandler(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteNaviProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Đang xóa mềm NaviProduct: {request.Id}");

        var product = await _repository.NaviProduct.GetProductByIdAsync(request.Id, trackChanges: true);
        if (product == null)
        {
            throw new KeyNotFoundException($"NaviProduct với Id: {request.Id} không tồn tại");
        }

        product.IsDelete = true;
        product.UDT = DateTime.Now;
        await _repository.SaveAsync();

        _logger.LogInfo($"Đã xóa mềm NaviProduct: {request.Id}");
        return Unit.Value;
    }
}

// ==================== NaviProductItem Command Handlers ====================

/// <summary>
/// Handler cho CreateNaviProductItemCommand
/// </summary>
public class CreateNaviProductItemHandler : IRequestHandler<CreateNaviProductItemCommand, NaviProductItemDto>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CreateNaviProductItemHandler(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<NaviProductItemDto> Handle(CreateNaviProductItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Đang tạo NaviProductItem: ProductId={request.ProductItemDto.ProductId}, ItemId={request.ProductItemDto.ItemId}");

        // Validate product exists
        var product = await _repository.NaviProduct.GetProductByIdAsync(request.ProductItemDto.ProductId);
        if (product == null)
        {
            throw new KeyNotFoundException($"NaviProduct với Id: {request.ProductItemDto.ProductId} không tồn tại");
        }

        // Validate item exists
        var item = await _repository.NaviItem.GetItemByIdAsync(request.ProductItemDto.ItemId);
        if (item == null)
        {
            throw new KeyNotFoundException($"NaviItem với Id: {request.ProductItemDto.ItemId} không tồn tại");
        }

        // Check if relationship already exists
        var exists = await _repository.NaviProductItem.ProductItemExistsAsync(
            request.ProductItemDto.ProductId, 
            request.ProductItemDto.ItemId);
        
        if (exists)
        {
            throw new InvalidOperationException($"Relationship giữa Product {request.ProductItemDto.ProductId} và Item {request.ProductItemDto.ItemId} đã tồn tại");
        }

        var productItem = _mapper.Map<NaviProductItem>(request.ProductItemDto);
        productItem.CDT = DateTime.Now;
        productItem.UDT = DateTime.Now;

        _repository.NaviProductItem.Create(productItem);
        await _repository.SaveAsync();

        var createdProductItem = await _repository.NaviProductItem.GetProductItemWithDetailsAsync(productItem.Id);
        
        _logger.LogInfo($"Đã tạo NaviProductItem với Id: {productItem.Id}");
        return _mapper.Map<NaviProductItemDto>(createdProductItem);
    }
}

/// <summary>
/// Handler cho DeleteNaviProductItemCommand
/// </summary>
public class DeleteNaviProductItemHandler : IRequestHandler<DeleteNaviProductItemCommand, Unit>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public DeleteNaviProductItemHandler(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteNaviProductItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Đang xóa mềm NaviProductItem: {request.Id}");

        var productItem = await _repository.NaviProductItem.GetProductItemByIdAsync(request.Id, trackChanges: true);
        if (productItem == null)
        {
            throw new KeyNotFoundException($"NaviProductItem với Id: {request.Id} không tồn tại");
        }

        productItem.IsDelete = true;
        productItem.UDT = DateTime.Now;
        await _repository.SaveAsync();

        _logger.LogInfo($"Đã xóa mềm NaviProductItem: {request.Id}");
        return Unit.Value;
    }
}
