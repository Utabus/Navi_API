using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Repository.Dapper;

/// <summary>
/// Factory tạo IDbConnection cho Dapper
/// </summary>
public class DapperContext
{
    private readonly string? _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    /// <summary>
    /// Tạo và mở connection mới
    /// </summary>
    public IDbConnection CreateConnection()
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' không được cấu hình. " +
                "Vui lòng thêm connection string vào appsettings.json");
        }

        return new SqlConnection(_connectionString);
    }

    /// <summary>
    /// Kiểm tra connection string có tồn tại không
    /// </summary>
    public bool HasConnectionString => !string.IsNullOrEmpty(_connectionString);
}
