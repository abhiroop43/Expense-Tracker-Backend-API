using ExpenseTracker.Application.Contracts.Persistence;
using FluentValidation;

namespace ExpenseTracker.Application.Features.Wallet.Commands.UpdateWallet;

public class UpdateWalletCommandValidator : AbstractValidator<UpdateWalletCommand>
{
    private readonly IWalletsRepository _walletsRepository;

    public UpdateWalletCommandValidator(IWalletsRepository walletsRepository)
    {
        _walletsRepository = walletsRepository;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters long.");

        RuleFor(x => x)
            .MustAsync(UniqueNameAsync)
            .WithMessage("{PropertyName must be unique.}");
    }

    private async Task<bool> UniqueNameAsync(UpdateWalletCommand request, CancellationToken cancellationToken)
    {
        return await _walletsRepository.IsUniqueWalletName(request.Name, cancellationToken);
    }
}