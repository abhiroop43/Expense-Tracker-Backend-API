using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Domain.Common;
using ExpenseTracker.Persistence.DatabaseContext;

namespace ExpenseTracker.Persistence.Repositories;

public class LookupsRepository<T>(ExpenseDbContext dbContext) : IGenericRepository<T> where T : BaseEntity
{
    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }
}