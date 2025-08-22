using ExpenseTracker.Application.Contracts.Identity;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace ExpenseTracker.Application.Features.Transaction.Commands.UploadReceipt;

public class UploadReceiptCommandValidator : AbstractValidator<UploadReceiptCommand>
{
    private const int MaxFileSizeInKb = 200;
    private readonly ILogger _logger;
    private readonly IUserService _userService;

    public UploadReceiptCommandValidator(ILogger logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
        RuleFor(x => x)
            .Must(MustBeAnImage)
            .Must(MustBeLessThanLimit)
            .WithMessage("Only images of type jpg, jpeg or png are allowed");
    }

    private static bool MustBeLessThanLimit(UploadReceiptCommand command)
    {
        return command.File.Length <= MaxFileSizeInKb * 1024;
    }

    private bool MustBeAnImage(UploadReceiptCommand command)
    {
        try
        {
            var extension = Path.GetExtension(command.FileName);

            if (string.IsNullOrEmpty(extension)) return false;

            if (extension.ToLower() != ".jpg" && extension.ToLower() != ".jpeg" && extension.ToLower() != ".png")
                return false;

            using var image = SKImage.FromEncodedData(command.File);
            return image != null;
        }
        catch (IOException ioException)
        {
            _logger.LogWarning(ioException, "File {0} uploaded by {1} is a corrupted stream", command.FileName,
                _userService.UserId);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error during file validation for file {0} uploaded by {1}",
                command.FileName, _userService.UserId);
            return false;
        }
    }
}