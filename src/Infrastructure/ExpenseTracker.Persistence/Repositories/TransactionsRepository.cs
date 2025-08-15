using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Domain;
using ExpenseTracker.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Repositories;

public class TransactionsRepository(ExpenseDbContext dbContext)
    : GenericRepository<Transaction>(dbContext), ITransactionsRepository
{
    public async Task<IReadOnlyList<Transaction>> GetTransactionsForUser(string? userId,
        CancellationToken cancellationToken = default)
    {
        if (DbContext.Transactions != null)
            return await DbContext.Transactions.Where(x => x.CreatedBy == userId).AsNoTracking()
                .ToListAsync(cancellationToken);

        return new List<Transaction>().AsReadOnly();
    }
}