using PointBoard;
using PointBoard.Host.Models.Comment;

namespace PointBoard.Host.Models.Point;

/// <summary>
/// Represents a detailed response model for a point, including comments.
/// </summary>
public class PointResponse : PointShortResponse
{
    /// <summary>
    /// Gets or sets the collection of comments associated with the point.
    /// </summary>
    public ICollection<CommentResponse> Comments { get; set; } = new List<CommentResponse>();

    /// <summary>
    /// Initializes a new instance of the <see cref="PointResponse"/> class.
    /// </summary>
    public PointResponse()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PointResponse"/> class from a <see cref="Core.Domain.Entities.Point"/> entity.
    /// </summary>
    /// <param name="point">The point entity.</param>
    public PointResponse(PointBoard.Core.Domain.Entities.Point point)
    {
        Id = point.Id;
        X = point.X;
        Y = point.Y;
        Radius = point.Radius;
        Color = point.Color;

        Comments = point.Comments.Select(comm => new CommentResponse
                         {
                             Id = comm.Id,
                             Text = comm.Text,
                             BackgroundColor = comm.BackgroundColor,
                             PointId = comm.PointId
                         })
                        .ToList();
    }
}