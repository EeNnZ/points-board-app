using PointBoard.Core.Abstractions;
using PointBoard.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace PointBoard.DataAccess.Repos;

public class EfRepo<T> : IRepo<T> where T : BaseEntity
{
    protected DbContext Context { get; }
    protected DbSet<T> DbSet { get; }

    public EfRepo(DbContext dbContext)
    {
        Context = dbContext;
        DbSet = Context.Set<T>();
    }

    public async Task<ICollection<T>> GetAllAsync()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Guid> CreateAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await SaveChanges();

        return entity.Id;
    }

    public async Task CreateManyAsync(ICollection<T> entities)
    {
        await DbSet.AddRangeAsync(entities);

        await SaveChanges();
    }

    public async Task<T?> UpdateAsync(T entity)
    {
        DbSet.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
        await SaveChanges();
        return await DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == entity.Id);
    }

    public async Task DeleteAsync(T entity)
    {
        T? entityEntry = await DbSet.FirstOrDefaultAsync(e => e.Id == entity.Id);

        if (entityEntry != null)
        {
            DbSet.Remove(entityEntry);
            await SaveChanges();
        }
    }

    private async Task SaveChanges()
    {
        await Context.SaveChangesAsync();
    }
}