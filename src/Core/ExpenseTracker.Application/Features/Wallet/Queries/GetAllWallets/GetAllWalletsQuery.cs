using MediatR;

namespace ExpenseTracker.Application.Features.Wallet.Queries.GetAllWallets;

public class GetAllWalletsQuery : IRequest<IReadOnlyList<WalletDto>>
{
}