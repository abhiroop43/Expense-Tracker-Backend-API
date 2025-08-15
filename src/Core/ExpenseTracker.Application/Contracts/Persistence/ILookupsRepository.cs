using ExpenseTracker.Domain;

namespace ExpenseTracker.Application.Contracts.Persistence;

public interface ILookupsRepository : IGenericRepository<Lookup>
{
    Task<bool> IsUniqueLookup(string lookupCode, string lookupTypeCode, CancellationToken cancellation = default);

    Task<bool> LookupExists(string lookupCode, string lookupTypeCode, CancellationToken cancellation = default);

    Task<IReadOnlyList<Lookup>> GetLookupsByType(string lookupTypeCode, CancellationToken cancellation = default);

    Task<bool> AddSeedData(List<Lookup> seedData, CancellationToken cancellation = default);
}