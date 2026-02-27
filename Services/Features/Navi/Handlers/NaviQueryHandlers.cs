using AutoMapper;
using LoggerService;
using MediatR;
using Repository.Contracts;
using Services.Features.Navi.Queries;
using Shared.DataTransferObjects;

namespace Services.Features.Navi.Handlers;

// ==================== NaviItem Query Handlers ====================

public class GetAllNaviItemsHandler : IRequestHandler<GetAllNaviItemsQuery, IEnumerable<NaviItemDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetAllNaviItemsHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NaviItemDto>> Handle(GetAllNaviItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.NaviItem.GetAllActiveItemsAsync(request.TrackChanges);
        return _mapper.Map<IEnumerable<NaviItemDto>>(items);
    }
}

public class GetNaviItemByIdHandler : IRequestHandler<GetNaviItemByIdQuery, NaviItemDto?>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetNaviItemByIdHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<NaviItemDto?> Handle(GetNaviItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repository.NaviItem.GetItemByIdAsync(request.Id, request.TrackChanges);
        return _mapper.Map<NaviItemDto?>(item);
    }
}

public class GetNaviItemWithProductsHandler : IRequestHandler<GetNaviItemWithProductsQuery, NaviItemDto?>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetNaviItemWithProductsHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<NaviItemDto?> Handle(GetNaviItemWithProductsQuery request, CancellationToken cancellationToken)
    {
        var item = await _repository.NaviItem.GetItemWithProductItemsAsync(request.Id, request.TrackChanges);
        return _mapper.Map<NaviItemDto?>(item);
    }
}

public class GetNaviItemsByTypeHandler : IRequestHandler<GetNaviItemsByTypeQuery, IEnumerable<NaviItemDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetNaviItemsByTypeHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NaviItemDto>> Handle(GetNaviItemsByTypeQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.NaviItem.GetItemsByTypeAsync(request.Type, request.TrackChanges);
        return _mapper.Map<IEnumerable<NaviItemDto>>(items);
    }
}

public class SearchNaviItemsHandler : IRequestHandler<SearchNaviItemsQuery, IEnumerable<NaviItemDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public SearchNaviItemsHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NaviItemDto>> Handle(SearchNaviItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.NaviItem.SearchItemsAsync(request.SearchTerm, request.TrackChanges);
        return _mapper.Map<IEnumerable<NaviItemDto>>(items);
    }
}

// ==================== NaviProduct Query Handlers ====================

public class GetAllNaviProductsHandler : IRequestHandler<GetAllNaviProductsQuery, IEnumerable<NaviProductDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetAllNaviProductsHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NaviProductDto>> Handle(GetAllNaviProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.NaviProduct.GetAllActiveProductsAsync(request.TrackChanges);
        return _mapper.Map<IEnumerable<NaviProductDto>>(products);
    }
}

public class GetNaviProductByIdHandler : IRequestHandler<GetNaviProductByIdQuery, NaviProductDto?>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetNaviProductByIdHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<NaviProductDto?> Handle(GetNaviProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.NaviProduct.GetProductByIdAsync(request.Id, request.TrackChanges);
        return _mapper.Map<NaviProductDto?>(product);
    }
}

public class GetNaviProductWithItemsHandler : IRequestHandler<GetNaviProductWithItemsQuery, NaviProductWithItemsDto?>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetNaviProductWithItemsHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<NaviProductWithItemsDto?> Handle(GetNaviProductWithItemsQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.NaviProduct.GetProductWithItemsAsync(request.Id, request.TrackChanges);
        return _mapper.Map<NaviProductWithItemsDto?>(product);
    }
}

public class SearchNaviProductsHandler : IRequestHandler<SearchNaviProductsQuery, IEnumerable<NaviProductDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public SearchNaviProductsHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NaviProductDto>> Handle(SearchNaviProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.NaviProduct.SearchProductsAsync(request.SearchTerm, request.TrackChanges);
        return _mapper.Map<IEnumerable<NaviProductDto>>(products);
    }
}

// ==================== NaviProductItem Query Handlers ====================

public class GetAllNaviProductItemsHandler : IRequestHandler<GetAllNaviProductItemsQuery, IEnumerable<NaviProductItemDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetAllNaviProductItemsHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NaviProductItemDto>> Handle(GetAllNaviProductItemsQuery request, CancellationToken cancellationToken)
    {
        var productItems = await _repository.NaviProductItem.GetAllActiveProductItemsAsync(request.TrackChanges);
        return _mapper.Map<IEnumerable<NaviProductItemDto>>(productItems);
    }
}

public class GetNaviProductItemByIdHandler : IRequestHandler<GetNaviProductItemByIdQuery, NaviProductItemDto?>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetNaviProductItemByIdHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<NaviProductItemDto?> Handle(GetNaviProductItemByIdQuery request, CancellationToken cancellationToken)
    {
        var productItem = await _repository.NaviProductItem.GetProductItemByIdAsync(request.Id, request.TrackChanges);
        return _mapper.Map<NaviProductItemDto?>(productItem);
    }
}

public class GetItemsByProductIdHandler : IRequestHandler<GetItemsByProductIdQuery, IEnumerable<NaviProductItemDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetItemsByProductIdHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NaviProductItemDto>> Handle(GetItemsByProductIdQuery request, CancellationToken cancellationToken)
    {
        var productItems = await _repository.NaviProductItem.GetItemsByProductIdAsync(request.ProductId, request.TrackChanges);
        return _mapper.Map<IEnumerable<NaviProductItemDto>>(productItems);
    }
}

public class GetProductsByItemIdHandler : IRequestHandler<GetProductsByItemIdQuery, IEnumerable<NaviProductItemDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetProductsByItemIdHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NaviProductItemDto>> Handle(GetProductsByItemIdQuery request, CancellationToken cancellationToken)
    {
        var productItems = await _repository.NaviProductItem.GetProductsByItemIdAsync(request.ItemId, request.TrackChanges);
        return _mapper.Map<IEnumerable<NaviProductItemDto>>(productItems);
    }
}

public class CheckProductItemExistsHandler : IRequestHandler<CheckProductItemExistsQuery, bool>
{
    private readonly IRepositoryManager _repository;

    public CheckProductItemExistsHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(CheckProductItemExistsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.NaviProductItem.ProductItemExistsAsync(request.ProductId, request.ItemId);
    }
}
