using PointBoard.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PointBoard.DataAccess.EntityConfigurations;

public class PointEntityConfiguration : IEntityTypeConfiguration<Point>
{
    public void Configure(EntityTypeBuilder<Point> b)
    {
        b.ToTable("Points");

        b.HasKey(point => point.Id);

        b.Property(point => point.X);
        b.Property(point => point.Y);
        b.Property(point => point.Radius);
        b.Property(point => point.Color)
         .HasDefaultValue("#000000")
         .HasMaxLength(7);
    }
}