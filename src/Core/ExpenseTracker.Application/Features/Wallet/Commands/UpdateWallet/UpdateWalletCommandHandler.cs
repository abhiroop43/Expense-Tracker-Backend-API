using AutoMapper;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Wallet.Commands.UpdateWallet;

public class UpdateWalletCommandHandler(
    IWalletsRepository walletsRepository,
    IMapper mapper,
    ILogger<UpdateWalletCommandHandler> logger) : IRequestHandler<UpdateWalletCommand, Unit>
{
    public async Task<Unit> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateWalletCommandValidator(walletsRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count > 0)
        {
            logger.LogWarning("Validation errors detected for {0}", nameof(request));
            throw new BadRequestException("Validation Errors", validationResult);
        }

        var existingWallet = await walletsRepository.GetByIdAsync(request.Id, cancellationToken);

        if (existingWallet == null)
        {
            logger.LogWarning("{0} with {1} was not found", nameof(existingWallet), request.Id);
            throw new NotFoundException(nameof(existingWallet), request.Id);
        }

        mapper.Map(request, existingWallet);

        await walletsRepository.UpdateAsync(existingWallet, cancellationToken);

        return Unit.Value;
    }
}