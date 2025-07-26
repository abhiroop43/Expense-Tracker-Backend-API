using MediatR;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Wallet.Queries.GetWalletById;

public class GetWalletByIdQuery : IRequest<WalletDetailDto>
{
    public ObjectId Id { get; set; }
}