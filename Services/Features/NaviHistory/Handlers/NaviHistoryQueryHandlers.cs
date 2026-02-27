using AutoMapper;
using MediatR;
using Repository.Contracts;
using Services.Features.Navi.Queries;
using Shared.DataTransferObjects;

namespace Services.Features.NaviHistory.Handlers;

// ==================== NaviHistory Query Handlers ====================

/// <summary>
/// Handler lấy tất cả NaviHistories chưa bị xóa
/// </summary>
public class GetAllNaviHistoriesHandler : IRequestHandler<GetAllNaviHistoriesQuery, IEnumerable<NaviHistoryDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetAllNaviHistoriesHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NaviHistoryDto>> Handle(GetAllNaviHistoriesQuery request, CancellationToken cancellationToken)
    {
        var histories = await _repository.NaviHistory.GetAllActiveHistoriesAsync(request.TrackChanges);
        return _mapper.Map<IEnumerable<NaviHistoryDto>>(histories);
    }
}

/// <summary>
/// Handler lấy NaviHistory theo Id
/// </summary>
public class GetNaviHistoryByIdHandler : IRequestHandler<GetNaviHistoryByIdQuery, NaviHistoryDto?>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetNaviHistoryByIdHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<NaviHistoryDto?> Handle(GetNaviHistoryByIdQuery request, CancellationToken cancellationToken)
    {
        var history = await _repository.NaviHistory.GetHistoryByIdAsync(request.Id, request.TrackChanges);
        return _mapper.Map<NaviHistoryDto?>(history);
    }
}

/// <summary>
/// Handler lấy NaviHistories theo mã nhân viên
/// </summary>
public class GetHistoriesByCodeNVHandler : IRequestHandler<GetHistoriesByCodeNVQuery, IEnumerable<NaviHistoryDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetHistoriesByCodeNVHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NaviHistoryDto>> Handle(GetHistoriesByCodeNVQuery request, CancellationToken cancellationToken)
    {
        var histories = await _repository.NaviHistory.GetHistoriesByCodeNVAsync(request.CodeNV, request.TrackChanges);
        return _mapper.Map<IEnumerable<NaviHistoryDto>>(histories);
    }
}

/// <summary>
/// Handler lấy NaviHistories theo ProductItem Id
/// </summary>
public class GetHistoriesByProductItemIdHandler : IRequestHandler<GetHistoriesByProductItemIdQuery, IEnumerable<NaviHistoryDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetHistoriesByProductItemIdHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NaviHistoryDto>> Handle(GetHistoriesByProductItemIdQuery request, CancellationToken cancellationToken)
    {
        var histories = await _repository.NaviHistory.GetHistoriesByProductItemIdAsync(request.ProductItemId, request.TrackChanges);
        return _mapper.Map<IEnumerable<NaviHistoryDto>>(histories);
    }
}

/// <summary>
/// Handler lấy NaviHistories theo Production Order
/// </summary>
public class GetHistoriesByPOHandler : IRequestHandler<GetHistoriesByPOQuery, IEnumerable<NaviHistoryDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetHistoriesByPOHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NaviHistoryDto>> Handle(GetHistoriesByPOQuery request, CancellationToken cancellationToken)
    {
        var histories = await _repository.NaviHistory.GetHistoriesByPOAsync(request.PO, request.TrackChanges);
        return _mapper.Map<IEnumerable<NaviHistoryDto>>(histories);
    }
}
