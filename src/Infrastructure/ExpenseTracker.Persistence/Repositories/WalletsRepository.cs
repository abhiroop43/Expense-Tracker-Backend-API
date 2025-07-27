using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Domain;
using ExpenseTracker.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Repositories;

public class WalletsRepository(ExpenseDbContext dbContext) : GenericRepository<Wallet>(dbContext), IWalletsRepository
{
    public async Task<IReadOnlyList<Wallet>> GetAllWalletsForUserAsync(string userId,
        CancellationToken cancellationToken)
    {
        if (DbContext.Wallets == null) return new List<Wallet>();

        return await DbContext.Wallets.Where(x => x.CreatedBy == userId).ToListAsync(cancellationToken);
    }

    public async Task<bool> IsUniqueWalletName(string name, string userId,
        CancellationToken cancellationToken = default)
    {
        if (DbContext.Wallets == null) return true;

        var wallet =
            await DbContext.Wallets.FirstOrDefaultAsync(
                x => x.Name.ToLower() == name.ToLower() && x.CreatedBy == userId, cancellationToken);

        return wallet == null;
    }
}