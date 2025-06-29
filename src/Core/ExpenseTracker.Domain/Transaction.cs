using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain;

public class Transaction : BaseEntity
{
    public required string TransactionTypeCode { get; set; }
    public required string WalletId { get; set; }
    public required string TransactionCategoryCode { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? ReceiptImageUrl { get; set; }
}