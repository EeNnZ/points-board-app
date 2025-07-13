namespace PointBoard.Host.Models.Point;

/// <summary>
/// Represents a short response model for a point.
/// </summary>
public class PointShortResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the point.
    /// </summary>
    public Guid Id { get; set; }

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
}