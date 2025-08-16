using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Contracts.Storage;
using ExpenseTracker.Application.Exceptions;
using ExpenseTracker.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Wallet.Commands.UploadWalletLogo;

public class UploadWalletLogoCommandHandler(
    ILogger<UploadWalletLogoCommandHandler> logger,
    IUserService userService,
    IBlobStorageService blobStorageService) : IRequestHandler<UploadWalletLogoCommand, string>
{
    public async Task<string> Handle(UploadWalletLogoCommand request, CancellationToken cancellationToken)
    {
        var validator = new UploadWalletLogoCommandValidator(logger, userService);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count <= 0)
            return await blobStorageService.UploadToBlobAsync(request.File, request.FileName,
                Constants.WalletImageTypeCode);

        logger.LogWarning("Validation error detected for {0}", nameof(request));
        throw new BadRequestException("Validation errors", validationResult);
    }
}