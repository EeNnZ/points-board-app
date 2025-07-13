using PointBoard;
using PointBoard.Host.Models.Point;

namespace PointBoard.Host.Models.Comment;

/// <summary>
/// Represents a detailed response model for a comment, including the related point.
/// </summary>
public class CommentResponse : CommentShortResponse
{
    /// <summary>
    /// Gets or sets the related point for the comment.
    /// </summary>
    public PointShortResponse? Point { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentResponse"/> class.
    /// </summary>
    public CommentResponse()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentResponse"/> class from a <see cref="Core.Domain.Entities.Comment"/> entity.
    /// </summary>
    /// <param name="comment">The comment entity.</param>
    public CommentResponse(PointBoard.Core.Domain.Entities.Comment comment)
    {
        Id = comment.Id;
        Point = new PointShortResponse
        {
            Id = comment.PointId,
            X = comment.Point.X,
            Y = comment.Point.Y,
            Radius = comment.Point.Radius,
            Color = comment.Point.Color
        };
    }
}