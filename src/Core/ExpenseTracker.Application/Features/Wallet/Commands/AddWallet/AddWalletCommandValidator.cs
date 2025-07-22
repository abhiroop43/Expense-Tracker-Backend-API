using ExpenseTracker.Application.Contracts.Persistence;
using FluentValidation;

namespace ExpenseTracker.Application.Features.Wallet.Commands.AddWallet;

public class AddWalletCommandValidator : AbstractValidator<AddWalletCommand>
{
    private readonly IWalletsRepository _walletsRepository;

    public AddWalletCommandValidator(IWalletsRepository walletsRepository)
    {
        _walletsRepository = walletsRepository;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters long.");

        RuleFor(x => x)
            .MustAsync(UniqueNameAsync)
            .WithMessage("{PropertyName} must be unique");
    }

    private async Task<bool> UniqueNameAsync(AddWalletCommand command, CancellationToken cancellation)
    {
        return await _walletsRepository.IsUniqueWalletName(command.Name, cancellation);
    }
}