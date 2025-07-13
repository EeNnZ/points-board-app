using PointBoard.Core.Domain.Entities;
using PointBoard.Host.Models.Comment;

namespace PointBoard.Host.Mappers;

/// <summary>
/// Provides mapping methods for comment models and entities.
/// </summary>
public class CommentMapper
{
    /// <summary>
    /// Maps a <see cref="CommentCreateOrUpdate"/> model and related point to a <see cref="Comment"/> entity.
    /// </summary>
    /// <param name="model">The comment creation or update model.</param>
    /// <param name="point">The related point entity.</param>
    /// <param name="comment">An optional existing comment entity to update.</param>
    /// <returns>The mapped <see cref="Comment"/> entity.</returns>
    public static Comment MapFromModel(CommentCreateOrUpdate model,
                                       Point point,
                                       Comment? comment = null)
    {
        throw new NotImplementedException();
    }
}