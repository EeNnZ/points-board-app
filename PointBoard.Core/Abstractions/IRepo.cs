using PointBoard.Core.Domain;

namespace PointBoard.Core.Abstractions;

/// <summary>
/// Defines a generic repository interface for managing entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">
/// The type of entity managed by the repository. Must inherit from <see cref="BaseEntity"/>.
/// </typeparam>
public interface IRepo<T> where T : BaseEntity
{
    Task<ICollection<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(T entity);
    Task CreateManyAsync(ICollection<T> entities);
    Task<T?> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}