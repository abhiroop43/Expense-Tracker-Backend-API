using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellation = default);
}