using MediatR;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Wallet.Commands.DeleteWallet;

public class DeleteWalletCommand : IRequest<Unit>
{
    public ObjectId Id { get; set; }
}