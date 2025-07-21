using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Domain;
using MongoDB.Bson;

namespace ExpenseTracker.Persistence.Repositories;

public class WalletsRepository : IWalletsRepository
{
    public async Task<Wallet> CreateAsync(Wallet entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Wallet> UpdateAsync(Wallet entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Wallet> DeleteAsync(Wallet entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Wallet?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Wallet>> GetAllAsync(CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsUniqueWalletName(string name, CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }
}