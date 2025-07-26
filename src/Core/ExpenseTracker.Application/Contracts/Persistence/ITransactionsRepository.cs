using ExpenseTracker.Domain;

namespace ExpenseTracker.Application.Contracts.Persistence;

public interface ITransactionsRepository : IGenericRepository<Transaction>
{
}