using AutoMapper;
using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Wallet.Queries.GetWalletById;

public class GetWalletByIdQueryHandler(
    IWalletsRepository walletsRepository,
    ILogger<GetWalletByIdQueryHandler> logger,
    IUserService userService,
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

        return mapper.Map<WalletDetailDto>(wallet);
    }
}