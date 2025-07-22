using MediatR;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Wallet.Commands.AddWallet;

public class AddWalletCommand : IRequest<ObjectId>
{
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
}