using ExpenseTracker.Domain;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Contracts.Persistence;

public interface IWalletsRepository : IGenericRepository<Wallet>
{
    Task<IReadOnlyList<Wallet>> GetAllWalletsForUserAsync(string userId, CancellationToken cancellationToken);
    Task<bool> IsUniqueWalletName(string name, string userId, CancellationToken cancellationToken = default);

    Task<bool> IsWalletPresentForUser(ObjectId walletId, string userId, CancellationToken cancellationToken);
}