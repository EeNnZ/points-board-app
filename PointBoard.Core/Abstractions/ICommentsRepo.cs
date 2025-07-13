using System.Linq.Expressions;
using PointBoard.Core.Domain.Entities;

namespace PointBoard.Core.Abstractions;

/// <summary>
/// Represents a repository interface for managing <see cref="Comment"/> entities.
/// </summary>
public interface ICommentsRepo : IRepo<Comment>
{
    /// <summary>
    /// Retrieves a collection of comments associated with a specific point.
    /// </summary>
    /// <param name="pointId">The unique identifier of the point.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="Comment"/> objects.</returns>
    Task<ICollection<Comment>> GetCommentsWithinPoint(Guid pointId);

    /// <summary>
    /// Retrieves all comments that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">An expression to filter the comments.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="Comment"/> objects that match the predicate.</returns>
    Task<ICollection<Comment>> GetAllAsync(Expression<Func<Comment, bool>> predicate);
}