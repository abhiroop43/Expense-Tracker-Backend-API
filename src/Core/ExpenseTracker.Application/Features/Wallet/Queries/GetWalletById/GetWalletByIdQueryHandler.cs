using AutoMapper;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Wallet.Queries.GetWalletById;

public class GetWalletByIdQueryHandler(
    IWalletsRepository walletsRepository,
    ILogger<GetWalletByIdQueryHandler> logger,
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

        return mapper.Map<WalletDetailDto>(wallet);
    }
}