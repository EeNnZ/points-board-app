namespace PointBoard.Host.Models.Comment;

/// <summary>
/// Represents a short response model for a comment.
/// </summary>
public class CommentShortResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the comment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the text of the comment.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the background color of the comment.
    /// </summary>
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the related point.
    /// </summary>
    public Guid? PointId { get; set; }
}