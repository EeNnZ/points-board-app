namespace PointBoard.Core.Domain.Entities;

/// <summary>
/// Represents a point entity with coordinates, radius, color, and associated comments.
/// </summary>
public class Point : BaseEntity
{
    /// <summary>
    /// Gets or sets the X coordinate of the point.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the point.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Gets or sets the radius of the point.
    /// </summary>
    public double Radius { get; set; }

    /// <summary>
    /// Gets or sets the color of the point.
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the collection of comments associated with the point.
    /// </summary>
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}