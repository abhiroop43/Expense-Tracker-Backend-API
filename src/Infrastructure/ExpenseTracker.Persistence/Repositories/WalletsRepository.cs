using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Domain;
using ExpenseTracker.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Repositories;

public class WalletsRepository(ExpenseDbContext dbContext) : GenericRepository<Wallet>(dbContext), IWalletsRepository
{
    public async Task<bool> IsUniqueWalletName(string name, CancellationToken cancellation = default)
    {
        if (DbContext.Wallets == null) return true;

        // check for CreatedBy user also, should be unique for the same user

        var wallet = await DbContext.Wallets.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower(), cancellation);

        return wallet == null;
    }
}