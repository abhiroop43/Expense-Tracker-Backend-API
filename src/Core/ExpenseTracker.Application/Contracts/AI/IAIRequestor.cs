using ExpenseTracker.Application.Models.AI;

namespace ExpenseTracker.Application.Contracts.AI;

public interface IAIRequestor
{
    Task<string> GetSuggestionsForSavingsAsync(IReadOnlyList<UserTransaction> transactions,
        CancellationToken cancellation = default);
}