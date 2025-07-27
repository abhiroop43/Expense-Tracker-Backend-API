using ExpenseTracker.Domain;

namespace ExpenseTracker.Application.Contracts.Persistence;

public interface IWalletsRepository : IGenericRepository<Wallet>
{
    Task<IReadOnlyList<Wallet>> GetAllWalletsForUserAsync(string userId, CancellationToken cancellationToken);
    Task<bool> IsUniqueWalletName(string name, string userId, CancellationToken cancellationToken = default);
}