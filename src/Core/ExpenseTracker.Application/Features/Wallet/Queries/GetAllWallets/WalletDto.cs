using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Wallet.Queries.GetAllWallets;

public class WalletDto
{
    public ObjectId Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
}