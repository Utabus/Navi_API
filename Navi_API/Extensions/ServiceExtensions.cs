using Entities;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Contracts;
using Repository.Dapper;
using Services;
using Services.Contracts;
using Shared.MappingProfiles;

namespace Navi_API.Extensions;

/// <summary>
/// Extension methods để cấu hình DI container
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Cấu hình Database Context
    /// Sử dụng SQL Server nếu có connection string, ngược lại dùng InMemory
    /// </summary>
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            // Development mode: sử dụng InMemory Database
            services.AddDbContext<RepositoryContext>(opts =>
                opts.UseInMemoryDatabase("NaviDB_InMemory"));
        }
        else
        {
            // Production mode: sử dụng SQL Server
            services.AddDbContext<RepositoryContext>(opts =>
                opts.UseSqlServer(connectionString, 
                    b => b.MigrationsAssembly("Navi_API")));
        }
    }

    /// <summary>
    /// Cấu hình Repository Manager
    /// </summary>
    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    /// <summary>
    /// Cấu hình Service Manager
    /// </summary>
    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();

    /// <summary>
    /// Cấu hình Logger Service
    /// </summary>
    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    /// <summary>
    /// Cấu hình Dapper Context (cho queries phức tạp)
    /// </summary>
    public static void ConfigureDapperContext(this IServiceCollection services) =>
        services.AddSingleton<DapperContext>();

    /// <summary>
    /// Cấu hình AutoMapper
    /// </summary>
    public static void ConfigureAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(typeof(MappingProfile));

    /// <summary>
    /// Cấu hình CORS
    /// </summary>
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
        });

    /// <summary>
    /// Cấu hình MediatR với tất cả handlers từ Services assembly
    /// </summary>
    public static void ConfigureMediatR(this IServiceCollection services) =>
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(Services.ServiceManager).Assembly));
}
