using AutoMapper;
using ExpenseTracker.Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Transaction.Queries.GetAllTransactions;

public class
    GetAllTransactionsQueryHandler(
        ITransactionsRepository transactionsRepository,
        IMapper mapper,
        ILogger<GetAllTransactionsQueryHandler> logger)
    : IRequestHandler<GetAllTransactionsQuery, IReadOnlyList<TransactionListDto>>
{
    public async Task<IReadOnlyList<TransactionListDto>> Handle(GetAllTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var transactions = await transactionsRepository.GetAllAsync(cancellationToken);

        if (transactions.Count == 0) logger.LogWarning("No transactions found for this user");

        return mapper.Map<IReadOnlyList<TransactionListDto>>(transactions);
    }
}