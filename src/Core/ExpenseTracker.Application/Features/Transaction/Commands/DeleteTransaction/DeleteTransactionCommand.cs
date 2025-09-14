using MediatR;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Transaction.Commands.DeleteTransaction;

public class DeleteTransactionCommand : IRequest<Unit>
{
    public ObjectId Id { get; set; }
}