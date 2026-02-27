using AutoMapper;
using LoggerService;
using Repository.Contracts;
using Services.Contracts;

namespace Services;

/// <summary>
/// Service Manager implementation
/// Note: Services are now handled via MediatR CQRS pattern
/// </summary>
public class ServiceManager : IServiceManager
{
    public ServiceManager(
        IRepositoryManager repositoryManager,
        ILoggerManager logger,
        IMapper mapper)
    {
        // Services are now handled via MediatR handlers
        // No direct service instances needed
    }
}
