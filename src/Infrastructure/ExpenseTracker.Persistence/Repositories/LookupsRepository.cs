using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Domain;
using ExpenseTracker.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Repositories;

public class LookupsRepository(ExpenseDbContext dbContext) : GenericRepository<Lookup>(dbContext), ILookupsRepository
{
    public async Task<bool> IsUniqueLookup(string lookupCode, string lookupTypeCode,
        CancellationToken cancellation = default)
    {
        if (DbContext.Lookups == null) return true;
        var existingLookup = await DbContext.Lookups.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Code.ToUpper() == lookupCode.ToUpper() &&
                x.LookupType.ToUpper() == lookupTypeCode.ToUpper(),
            cancellation);

        return existingLookup == null;
    }

    public async Task<IReadOnlyList<Lookup>> GetLookupsByType(string lookupTypeCode,
        CancellationToken cancellation = default)
    {
        if (DbContext.Lookups == null) return new List<Lookup>();
        var lookups = await DbContext.Lookups.AsNoTracking()
            .Where(x => x.LookupType.ToUpper() == lookupTypeCode.ToUpper()).ToListAsync(cancellation);

        return lookups;
    }

    public async Task<bool> AddSeedData(List<Lookup> seedData, CancellationToken cancellation = default)
    {
        if (DbContext.Lookups == null) return false;

        await DbContext.Lookups.AddRangeAsync(seedData, cancellation);
        var rowsSaved = await DbContext.SaveChangesAsync(cancellation);
        return rowsSaved > 0;
    }
}