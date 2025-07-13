using PointBoard.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PointBoard.DataAccess.EntityConfigurations;

public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> b)
    {
        b.ToTable("Comments");

        b.HasKey(comment => comment.Id);

        b.Property(comment => comment.BackgroundColor)
         .HasDefaultValue("#FFFFFF")
         .HasMaxLength(7);
        b.Property(comment => comment.Text);

        b.HasOne<Point>(comment => comment.Point)
         .WithMany(comment => comment.Comments)
         .HasForeignKey(comment => comment.PointId);
    }
}