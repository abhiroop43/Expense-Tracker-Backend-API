using ExpenseTracker.Application.Contracts.Identity;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace ExpenseTracker.Application.Features.Wallet.Commands.UploadWalletLogo;

public class UploadWalletLogoCommandValidator : AbstractValidator<UploadWalletLogoCommand>
{
    private readonly ILogger _logger;
    private readonly IUserService _userService;

    public UploadWalletLogoCommandValidator(ILogger logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
        RuleFor(x => x)
            .Must(MustBeAnImage)
            .Must(MustBeLessThanLimit)
            .WithMessage("Only images of type jpg, jpeg or png are allowed");
    }

    private static bool MustBeLessThanLimit(UploadWalletLogoCommand command)
    {
        return command.File.Length <= 200 * 1024;
    }

    private bool MustBeAnImage(UploadWalletLogoCommand command)
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