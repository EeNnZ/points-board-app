namespace PointBoard.Core.Domain;

public class BaseEntity
{
    /// <summary>
    ///     Gets the globally unique identifier object.
    ///     This GUID is automatically computed by the database and can be used to uniquely identify objects across different
    ///     systems.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
}