using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Contracts.Storage;
using ExpenseTracker.Application.Exceptions;
using ExpenseTracker.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Transaction.Commands.UploadReceipt;

public class UploadReceiptCommandHandler(
    ILogger<UploadReceiptCommandHandler> logger,
    IUserService userService,
    IBlobStorageService blobStorageService) : IRequestHandler<UploadReceiptCommand, string>
{
    public async Task<string> Handle(UploadReceiptCommand request, CancellationToken cancellationToken)
    {
        var validator = new UploadReceiptCommandValidator(logger, userService);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count <= 0)
            return await blobStorageService.UploadToBlobAsync(request.File, request.FileName,
                Constants.TransactionReceiptImageTypeCode);

        logger.LogWarning("Validation error detected for {0}", nameof(request));
        throw new BadRequestException("Validation errors", validationResult);
    }
}