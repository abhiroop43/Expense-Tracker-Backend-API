using AutoMapper;
using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Contracts.Storage;
using ExpenseTracker.Application.Exceptions;
using ExpenseTracker.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Wallet.Queries.GetAllWallets;

public class GetAllWalletsQueryHandler(
    IWalletsRepository walletsRepository,
    ILogger<GetAllWalletsQueryHandler> logger,
    IMapper mapper,
    IBlobStorageService blobStorageService,
    IUserService userService) : IRequestHandler<GetAllWalletsQuery, IReadOnlyList<WalletDto>>
{
    public async Task<IReadOnlyList<WalletDto>> Handle(GetAllWalletsQuery request, CancellationToken cancellationToken)
    {
        var userId = userService.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogWarning("No userId defined, therefore preventing access to wallets");
            throw new ForbiddenException("You are not authorized to access this resource");
        }

        var wallets = await walletsRepository.GetAllWalletsForUserAsync(userId, cancellationToken);

        if (wallets.Count == 0) logger.LogWarning("No wallets saved for this user");

        var walletsDto = mapper.Map<IReadOnlyList<WalletDto>>(wallets);

        foreach (var walletDto in walletsDto)
            walletDto.ImageUrl =
                blobStorageService.GetBlobSasUrl(walletDto.ImageUrl, Constants.WalletImageTypeCode);

        return walletsDto;
    }
}