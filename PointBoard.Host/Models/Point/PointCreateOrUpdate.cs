namespace PointBoard.Host.Models.Point;

/// <summary>
/// Represents a model for creating or updating a point.
/// </summary>
public class PointCreateOrUpdate
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
}