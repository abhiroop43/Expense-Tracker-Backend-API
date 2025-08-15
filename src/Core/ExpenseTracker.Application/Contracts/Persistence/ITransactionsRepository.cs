using ExpenseTracker.Domain;

namespace ExpenseTracker.Application.Contracts.Persistence;

public interface ITransactionsRepository : IGenericRepository<Transaction>
{
    Task<IReadOnlyList<Transaction>> GetTransactionsForUser(string? userId,
        CancellationToken cancellationToken = default);
}