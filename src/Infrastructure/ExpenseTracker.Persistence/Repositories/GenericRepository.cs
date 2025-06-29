using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace ExpenseTracker.Persistence.Repositories;

public class GenericRepository<T>(ExpenseDbContext dbContext) : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ExpenseDbContext DbContext = dbContext;

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbContext.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Entry(entity).State = EntityState.Modified;
        await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<T?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellation = default)
    {
        return await DbContext.Set<T>().AsNoTracking().ToListAsync(cancellation);
    }
}