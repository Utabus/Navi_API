using Entities;
using LoggerService;
using Navi_API.Extensions;
using Navi_API.Filters;
using Serilog;

// Cấu hình Serilog từ appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .Build())
    .CreateLogger();

try
{
    Log.Information("Đang khởi động ứng dụng...");

    var builder = WebApplication.CreateBuilder(args);

    // Sử dụng Serilog thay cho built-in logging
    builder.Host.UseSerilog();

    // Cấu hình các services
    builder.Services.ConfigureCors();
    builder.Services.ConfigureLoggerService();
    builder.Services.ConfigureSqlContext(builder.Configuration);
    builder.Services.ConfigureRepositoryManager();
    builder.Services.ConfigureServiceManager();
    builder.Services.ConfigureDapperContext();
    builder.Services.ConfigureAutoMapper();
    builder.Services.ConfigureMediatR();

    // Add controllers với global filters
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ApiExceptionFilter>();
        // Bỏ comment dòng dưới nếu muốn auto-wrap tất cả responses
        // options.Filters.Add<ApiResponseWrapperFilter>();
    });
    
    // Swagger/OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() 
        { 
            Title = "Navi API", 
            Version = "v1",
            Description = "Clean Architecture API với Entity Framework và Dapper",
            Contact = new() { Name = "Navi Team", Email = "support@navi.com" }
        });
        
        // Đọc XML documentation
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    });

    var app = builder.Build();

    // Seed data cho InMemory Database
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
        context.Database.EnsureCreated();
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
     
    }
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Navi API v1"));
    app.UseCors("CorsPolicy");
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    Log.Information("Ứng dụng đã khởi động thành công!");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Ứng dụng bị dừng do lỗi nghiêm trọng!");
}
finally
{
    Log.CloseAndFlush();
}
