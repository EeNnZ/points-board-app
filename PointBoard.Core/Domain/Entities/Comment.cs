namespace PointBoard.Core.Domain.Entities;

/// <summary>
/// Represents a comment entity associated with a specific point.
/// </summary>
public class Comment : BaseEntity
{
    /// <summary>
    /// Gets or sets the text content of the comment.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the background color of the comment, represented as a string (e.g., hex code).
    /// </summary>
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated point.
    /// </summary>
    public Guid PointId { get; set; }

    /// <summary>
    /// Gets or sets the point entity to which this comment belongs.
    /// </summary>
    public virtual Point Point { get; set; } = null!;
}