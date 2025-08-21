namespace ExpenseTracker.Application.Features.Wallet.Queries.GetAllWallets;

public class WalletDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
}