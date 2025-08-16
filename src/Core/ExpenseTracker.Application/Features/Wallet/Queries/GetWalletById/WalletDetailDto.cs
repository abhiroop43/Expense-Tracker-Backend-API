namespace ExpenseTracker.Application.Features.Wallet.Queries.GetWalletById;

public class WalletDetailDto
{
    public string Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
}