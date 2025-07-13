using System.Linq.Expressions;
using PointBoard.Core.Abstractions;
using PointBoard.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PointBoard.DataAccess.Repos;

public class CommentsEfRepo : EfRepo<Comment>, ICommentsRepo
{
    public CommentsEfRepo(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ICollection<Comment>> GetCommentsWithinPoint(Guid pointId)
    {
        return await DbSet.Where(comment => comment.PointId == pointId)
                          .Include(comment => comment.Point)
                          .ToListAsync();
    }

    public async Task<ICollection<Comment>> GetAllAsync(Expression<Func<Comment, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }
}