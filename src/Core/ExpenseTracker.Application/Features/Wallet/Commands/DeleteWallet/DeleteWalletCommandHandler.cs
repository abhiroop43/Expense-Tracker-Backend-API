using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Wallet.Commands.DeleteWallet;

public class DeleteWalletCommandHandler(
    IWalletsRepository walletsRepository,
    ILogger<DeleteWalletCommandHandler> logger) : IRequestHandler<DeleteWalletCommand, Unit>
{
    public async Task<Unit> Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await walletsRepository.GetByIdAsync(request.Id, cancellationToken);

        if (wallet == null)
        {
            logger.LogWarning("Unable to find wallet with id: {0}", request.Id);
            throw new NotFoundException(nameof(wallet), request.Id);
        }

        await walletsRepository.DeleteAsync(wallet, cancellationToken);

        return Unit.Value;
    }
}