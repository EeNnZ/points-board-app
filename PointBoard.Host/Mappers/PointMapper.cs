using PointBoard.Core.Domain.Entities;
using PointBoard.Host.Models.Point;

namespace PointBoard.Host.Mappers;

/// <summary>
/// Provides mapping methods for point models and entities.
/// </summary>
public class PointMapper
{
    /// <summary>
    /// Maps a <see cref="PointCreateOrUpdate"/> model and related comment to a <see cref="Point"/> entity.
    /// </summary>
    /// <param name="model">The point creation or update model.</param>
    /// <param name="comment">The related comment entity.</param>
    /// <param name="point">An optional existing point entity to update.</param>
    /// <returns>The mapped <see cref="Point"/> entity.</returns>
    public static Point MapFromModel(PointCreateOrUpdate model,
                                     Comment comment,
                                     Point? point = null)
    {
        throw new NotImplementedException();
    }
}