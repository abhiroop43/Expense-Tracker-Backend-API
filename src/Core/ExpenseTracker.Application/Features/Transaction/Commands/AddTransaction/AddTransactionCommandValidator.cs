using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Domain.Common;
using FluentValidation;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Transaction.Commands.AddTransaction;

public class AddTransactionCommandValidator : AbstractValidator<AddTransactionCommand>
{
    private const string RequiredFieldErrorMessage = "{PropertyName} is required";
    private readonly ILookupsRepository _lookupsRepository;
    private readonly IUserService _userService;
    private readonly IWalletsRepository _walletsRepository;

    public AddTransactionCommandValidator(ILookupsRepository lookupsRepository, IWalletsRepository walletsRepository,
        IUserService userService)
    {
        _lookupsRepository = lookupsRepository;
        _walletsRepository = walletsRepository;
        _userService = userService;


        RuleFor(x => x.TransactionTypeCode)
            .NotEmpty()
            .WithMessage(RequiredFieldErrorMessage)
            .MustAsync(TransactionTypeMustExistAsync)
            .WithMessage("No transaction type exists with this code");

        RuleFor(x => x.TransactionCategoryCode)
            .NotEmpty()
            .WithMessage(RequiredFieldErrorMessage)
            .MustAsync(TransactionCategoryMustExistAsync)
            .WithMessage("No transaction category exists with this code");

        RuleFor(x => x.WalletId)
            .NotEmpty()
            .WithMessage(RequiredFieldErrorMessage)
            .MustAsync(WalletMustExistAsync)
            .WithMessage("No wallet with this Id exists for user");

        RuleFor(x => x.TransactionDate)
            .NotEmpty()
            .WithMessage(RequiredFieldErrorMessage);

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage(RequiredFieldErrorMessage)
            .GreaterThan(0)
            .WithMessage("{PropertyName} must be greater than 0");
    }

    private async Task<bool> WalletMustExistAsync(ObjectId? walletId, CancellationToken cancellationToken)
    {
        return await _walletsRepository.IsWalletPresentForUser(walletId!.Value, _userService.UserId!,
            cancellationToken);
    }

    private async Task<bool> TransactionCategoryMustExistAsync(string transactionCategoryCode,
        CancellationToken cancellationToken)
    {
        return await _lookupsRepository.LookupExists(transactionCategoryCode, Constants.TransactionCategory,
            cancellationToken);
    }

    private async Task<bool> TransactionTypeMustExistAsync(string transactionTypeCode,
        CancellationToken cancellationToken)
    {
        return await _lookupsRepository.LookupExists(transactionTypeCode, Constants.TransactionType, cancellationToken);
    }
}