using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Domain;
using ExpenseTracker.Persistence.DatabaseContext;

namespace ExpenseTracker.Persistence.Repositories;

public class TransactionsRepository(ExpenseDbContext dbContext)
    : GenericRepository<Transaction>(dbContext), ITransactionsRepository
{
}