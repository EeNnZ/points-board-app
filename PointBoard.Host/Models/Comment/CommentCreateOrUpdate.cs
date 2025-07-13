namespace PointBoard.Host.Models.Comment;

/// <summary>
/// Represents a model for creating or updating a comment.
/// </summary>
public class CommentCreateOrUpdate
{
    /// <summary>
    /// Gets or sets the identifier of the related point.
    /// </summary>
    public Guid PointId { get; set; }

    /// <summary>
    /// Gets or sets the text of the comment.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the background color of the comment.
    /// </summary>
    public string? BackgroundColor { get; set; }
}