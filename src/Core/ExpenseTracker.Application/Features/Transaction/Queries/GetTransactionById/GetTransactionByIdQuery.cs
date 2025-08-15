using MediatR;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Transaction.Queries.GetTransactionById;

public class GetTransactionByIdQuery : IRequest<TransactionDetailDto>
{
    public ObjectId Id { get; set; }
}