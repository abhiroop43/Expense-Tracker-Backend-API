using AutoMapper;
using ExpenseTracker.Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Wallet.Queries.GetAllWallets;

public class GetAllWalletsQueryHandler(
    IWalletsRepository walletsRepository,
    ILogger<GetAllWalletsQueryHandler> logger,
    IMapper mapper) : IRequestHandler<GetAllWalletsQuery, IReadOnlyList<WalletDto>>
{
    public async Task<IReadOnlyList<WalletDto>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
    {
        var wallets = await walletsRepository.GetAllAsync(cancellationToken);

        if (wallets.Count == 0) logger.LogWarning("No wallets saved for this user");

        return mapper.Map<IReadOnlyList<WalletDto>>(wallets);
    }
}