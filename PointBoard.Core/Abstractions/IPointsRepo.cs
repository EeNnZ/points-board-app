using System.Linq.Expressions;
using PointBoard.Core.Domain.Entities;

namespace PointBoard.Core.Abstractions;

/// <summary>
/// Represents a repository interface for managing <see cref="Point"/> entities.
/// </summary>
/// <remarks>
/// Extends the <see cref="IRepo{Point}"/> interface with additional methods specific to <see cref="Point"/>.
/// </remarks>
public interface IPointsRepo : IRepo<Point>
{
    /// <summary>
    /// Asynchronously retrieves a single <see cref="Point"/> entity that matches the specified predicate.
    /// </summary>
    /// <param name="predicate">An expression to filter the <see cref="Point"/> entities.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the matching <see cref="Point"/>, or <c>null</c> if no match is found.
    /// </returns>
    Task<Point?> GetPointAsync(Expression<Func<Point, bool>> predicate);

    /// <summary>
    /// Asynchronously retrieves a collection of <see cref="Point"/> entities that have associated comments.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection of <see cref="Point"/> entities with comments.
    /// </returns>
    Task<ICollection<Point>> GetPointsWithCommentsAsync();
}