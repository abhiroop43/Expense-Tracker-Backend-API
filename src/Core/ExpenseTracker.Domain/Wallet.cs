using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain;

public class Wallet : BaseEntity
{
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
}