using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Wallet.Queries.GetWalletById;

public class WalletDetailDto
{
    public ObjectId Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
}