using AutoMapper;
using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Contracts.Storage;
using ExpenseTracker.Application.Exceptions;
using ExpenseTracker.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Wallet.Queries.GetWalletById;

public class GetWalletByIdQueryHandler(
    IWalletsRepository walletsRepository,
    ILogger<GetWalletByIdQueryHandler> logger,
    IUserService userService,
    IBlobStorageService blobStorageService,
    IMapper mapper) : IRequestHandler<GetWalletByIdQuery, WalletDetailDto>
{
    public async Task<WalletDetailDto> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
    {
        var wallet = await walletsRepository.GetByIdAsync(request.Id, cancellationToken);

        if (wallet == null)
        {
            logger.LogWarning("No {0} found with {1}", nameof(wallet), request.Id);
            throw new NotFoundException(nameof(wallet), request.Id);
        }

        if (wallet.CreatedBy != userService.UserId)
        {
            logger.LogWarning("Unauthorized access to wallet {0} by user {1}", wallet.Id, userService.UserId);
            throw new ForbiddenException("You are not authorized to access this resource");
        }

        var walletDto = mapper.Map<WalletDetailDto>(wallet);

        walletDto.ImageUrl = blobStorageService.GetBlobSasUrl(walletDto.ImageUrl, Constants.WalletImageTypeCode);

        return walletDto;
    }
}