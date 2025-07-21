using ExpenseTracker.Application.Models.AI;

namespace ExpenseTracker.Application.Contracts.AI;

public interface IAiRequestor
{
    Task<string> GetSuggestionsForSavingsAsync(IReadOnlyList<UserTransaction> transactions,
        CancellationToken cancellation = default);
}