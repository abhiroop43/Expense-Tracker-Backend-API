using MediatR;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Wallet.Commands.UpdateWallet;

public class UpdateWalletCommand : IRequest<Unit>
{
    public ObjectId Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
}