using System.Linq.Expressions;

namespace Repository.Contracts;

/// <summary>
/// Generic repository interface với các operations CRUD cơ bản
/// </summary>
/// <typeparam name="T">Entity type phải là class</typeparam>
public interface IRepositoryBase<T> where T : class
{
    /// <summary>
    /// Lấy tất cả entities (không tracking)
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false);

    /// <summary>
    /// Lấy tất cả entities theo điều kiện
    /// </summary>
    Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges = false);

    /// <summary>
    /// Lấy entity theo Id
    /// </summary>
    Task<T?> GetByIdAsync(Guid id, bool trackChanges = false);

    /// <summary>
    /// Tạo entity mới
    /// </summary>
    void Create(T entity);

    /// <summary>
    /// Cập nhật entity
    /// </summary>
    void Update(T entity);

    /// <summary>
    /// Xóa entity
    /// </summary>
    void Delete(T entity);

    /// <summary>
    /// Tạo nhiều entities
    /// </summary>
    void CreateRange(IEnumerable<T> entities);

    /// <summary>
    /// Xóa nhiều entities
    /// </summary>
    void DeleteRange(IEnumerable<T> entities);
}
