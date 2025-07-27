using AutoMapper;
using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Wallet.Commands.AddWallet;

public class AddWalletCommandHandler(
    IWalletsRepository walletsRepository,
    ILogger<AddWalletCommandHandler> logger,
    IUserService userService,
    IMapper mapper)
    : IRequestHandler<AddWalletCommand, ObjectId>
{
    public async Task<ObjectId> Handle(AddWalletCommand request, CancellationToken cancellationToken)
    {
        var validator = new AddWalletCommandValidator(walletsRepository, userService);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Any())
        {
            logger.LogWarning("Validation errors detected in Add Wallet for {0}", nameof(request));
            throw new BadRequestException("Validation errors", validationResult);
        }

        var wallet = mapper.Map<Domain.Wallet>(request);

        var savedData = await walletsRepository.CreateAsync(wallet, cancellationToken);

        return savedData.Id;
    }
}