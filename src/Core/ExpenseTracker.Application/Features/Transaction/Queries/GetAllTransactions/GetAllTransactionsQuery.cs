using MediatR;

namespace ExpenseTracker.Application.Features.Transaction.Queries.GetAllTransactions;

public class GetAllTransactionsQuery : IRequest<IReadOnlyList<TransactionListDto>>
{
}