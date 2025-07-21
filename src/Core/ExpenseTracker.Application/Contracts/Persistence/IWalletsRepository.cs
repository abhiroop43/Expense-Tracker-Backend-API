using ExpenseTracker.Domain;

namespace ExpenseTracker.Application.Contracts.Persistence;

public interface IWalletsRepository : IGenericRepository<Wallet>
{
    Task<bool> IsUniqueWalletName(string name, CancellationToken cancellation);
}