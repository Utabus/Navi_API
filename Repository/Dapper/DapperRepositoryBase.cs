using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Dapper;
using Entities.Models;
using Repository.Contracts;

namespace Repository.Dapper;

/// <summary>
/// Generic Dapper repository với CRUD operations cơ bản
/// Tự động generate SQL queries từ entity properties
/// </summary>
/// <typeparam name="T">Entity type phải kế thừa BaseEntity</typeparam>
public abstract class DapperRepositoryBase<T> : IRepositoryBase<T> where T : BaseEntity
{
    protected readonly DapperContext _context;
    protected readonly string _tableName;
    protected readonly IEnumerable<PropertyInfo> _properties;

    protected DapperRepositoryBase(DapperContext context, string? tableName = null)
    {
        _context = context;
        _tableName = tableName ?? typeof(T).Name + "s";
        _properties = typeof(T).GetProperties()
            .Where(p => p.CanRead && p.CanWrite && !IsNavigationProperty(p));
    }

    private static bool IsNavigationProperty(PropertyInfo property)
    {
        // Bỏ qua navigation properties và collections
        if (property.PropertyType.IsClass && 
            property.PropertyType != typeof(string) &&
            !property.PropertyType.IsArray)
        {
            return true;
        }
        
        if (property.PropertyType.IsGenericType &&
            property.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
        {
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false)
    {
        var sql = $"SELECT * FROM {_tableName}";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<T>(sql);
    }

    /// <inheritdoc/>
    public async Task<T?> GetByIdAsync(Guid id, bool trackChanges = false)
    {
        var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        // Dapper không hỗ trợ Expression trực tiếp
        // Cần override trong class con với custom SQL
        var sql = $"SELECT * FROM {_tableName}";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<T>(sql);
    }

    /// <inheritdoc/>
    public void Create(T entity)
    {
        var columns = _properties
            .Where(p => p.Name != "Id" || entity.Id != Guid.Empty)
            .Select(p => p.Name);
        
        var parameters = _properties
            .Where(p => p.Name != "Id" || entity.Id != Guid.Empty)
            .Select(p => "@" + p.Name);

        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }

        var sql = $"INSERT INTO {_tableName} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", parameters)})";
        
        using var connection = _context.CreateConnection();
        connection.Execute(sql, entity);
    }

    /// <inheritdoc/>
    public void Update(T entity)
    {
        var setClause = _properties
            .Where(p => p.Name != "Id")
            .Select(p => $"{p.Name} = @{p.Name}");

        entity.UpdatedAt = DateTime.UtcNow;

        var sql = $"UPDATE {_tableName} SET {string.Join(", ", setClause)} WHERE Id = @Id";
        
        using var connection = _context.CreateConnection();
        connection.Execute(sql, entity);
    }

    /// <inheritdoc/>
    public void Delete(T entity)
    {
        var sql = $"DELETE FROM {_tableName} WHERE Id = @Id";
        using var connection = _context.CreateConnection();
        connection.Execute(sql, new { entity.Id });
    }

    /// <inheritdoc/>
    public void CreateRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            Create(entity);
        }
    }

    /// <inheritdoc/>
    public void DeleteRange(IEnumerable<T> entities)
    {
        var ids = entities.Select(e => e.Id).ToList();
        var sql = $"DELETE FROM {_tableName} WHERE Id IN @Ids";
        using var connection = _context.CreateConnection();
        connection.Execute(sql, new { Ids = ids });
    }

    /// <summary>
    /// Execute custom query
    /// </summary>
    protected async Task<IEnumerable<T>> QueryAsync(string sql, object? parameters = null)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<T>(sql, parameters);
    }

    /// <summary>
    /// Execute custom query với join
    /// </summary>
    protected async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object? parameters = null)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<TResult>(sql, parameters);
    }
}
