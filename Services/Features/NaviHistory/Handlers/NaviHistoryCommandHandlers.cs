using AutoMapper;
using MediatR;
using Repository.Contracts;
using Services.Features.Navi.Commands;
using Shared.DataTransferObjects;

namespace Services.Features.NaviHistory.Handlers;

// ==================== NaviHistory Command Handlers ====================

/// <summary>
/// Handler tạo NaviHistory record mới
/// </summary>
public class CreateNaviHistoryHandler : IRequestHandler<CreateNaviHistoryCommand, NaviHistoryDto>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public CreateNaviHistoryHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<NaviHistoryDto> Handle(CreateNaviHistoryCommand request, CancellationToken cancellationToken)
    {
        var historyEntity = _mapper.Map<Entities.Models.NaviHistory>(request.Dto);
        historyEntity.CDT = DateTime.Now;
        historyEntity.UDT = DateTime.Now;
        historyEntity.IsDelete = false;

        _repository.NaviHistory.Create(historyEntity);
        await _repository.SaveAsync();

        return _mapper.Map<NaviHistoryDto>(historyEntity);
    }
}

/// <summary>
/// Handler cập nhật NaviHistory record
/// </summary>
public class UpdateNaviHistoryHandler : IRequestHandler<UpdateNaviHistoryCommand, Unit>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public UpdateNaviHistoryHandler(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateNaviHistoryCommand request, CancellationToken cancellationToken)
    {
        var historyEntity = await _repository.NaviHistory.GetHistoryByIdAsync(request.Id, trackChanges: true);

        if (historyEntity is null)
            throw new KeyNotFoundException($"NaviHistory với Id: {request.Id} không tồn tại.");

        _mapper.Map(request.Dto, historyEntity);
        historyEntity.UDT = DateTime.Now;

        await _repository.SaveAsync();
        return Unit.Value;
    }
}

/// <summary>
/// Handler xóa mềm NaviHistory record
/// </summary>
public class DeleteNaviHistoryHandler : IRequestHandler<DeleteNaviHistoryCommand, Unit>
{
    private readonly IRepositoryManager _repository;

    public DeleteNaviHistoryHandler(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteNaviHistoryCommand request, CancellationToken cancellationToken)
    {
        var historyEntity = await _repository.NaviHistory.GetHistoryByIdAsync(request.Id, trackChanges: true);

        if (historyEntity is null)
            throw new KeyNotFoundException($"NaviHistory với Id: {request.Id} không tồn tại.");

        historyEntity.IsDelete = true;
        historyEntity.UDT = DateTime.Now;

        await _repository.SaveAsync();
        return Unit.Value;
    }
}
