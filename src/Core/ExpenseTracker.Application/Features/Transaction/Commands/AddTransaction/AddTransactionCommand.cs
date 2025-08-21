using MediatR;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Transaction.Commands.AddTransaction;

public class AddTransactionCommand : IRequest<ObjectId>
{
    public string TransactionTypeCode { get; set; } = default!;
    public ObjectId? WalletId { get; set; }
    public string TransactionCategoryCode { get; set; } = default!;
    public DateTime? TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? ReceiptImageUrl { get; set; }
}