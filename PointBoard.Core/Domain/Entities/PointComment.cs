namespace PointBoard.Core.Domain.Entities;

public class PointComment : BaseEntity
{
    public Guid PointId { get; set; }
    public virtual Point? Point { get; set; }

    public Guid CommentId { get; set; }
    public virtual Comment? Comment { get; set; }
}