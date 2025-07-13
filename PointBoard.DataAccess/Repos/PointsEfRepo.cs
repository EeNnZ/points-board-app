using System.Linq.Expressions;
using PointBoard.Core.Abstractions;
using PointBoard.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PointBoard.DataAccess.Repos;

public class PointsEfRepo : EfRepo<Point>, IPointsRepo
{
    public PointsEfRepo(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Point?> GetPointAsync(Expression<Func<Point, bool>> predicate)
    {
        return await DbSet.AsNoTracking().SingleOrDefaultAsync(predicate);
    }

    public async Task<ICollection<Point>> GetPointsWithCommentsAsync()
    {
        return await DbSet.Where(point => point.Comments.Any()).ToListAsync();
    }
}