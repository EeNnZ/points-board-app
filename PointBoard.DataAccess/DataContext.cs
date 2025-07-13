using PointBoard.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PointBoard.DataAccess.EntityConfigurations;

namespace PointBoard.DataAccess;

public class DataContext : DbContext
{
    public DbSet<Point> Points { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;

    public DataContext()
    {
    }

    // public DataContext(DbContextOptions<DataContext> options)
    //     : base(options)
    // {
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("PointBoardDb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PointEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CommentEntityConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}