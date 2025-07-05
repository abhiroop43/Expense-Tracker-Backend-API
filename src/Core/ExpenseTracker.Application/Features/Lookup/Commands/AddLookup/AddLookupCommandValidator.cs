using ExpenseTracker.Application.Contracts.Persistence;
using FluentValidation;

namespace ExpenseTracker.Application.Features.Lookup.Commands.AddLookup;

public class AddLookupCommandValidator : AbstractValidator<AddLookupCommand>
{
    private readonly ILookupsRepository _lookupsRepository;

    public AddLookupCommandValidator(ILookupsRepository lookupsRepository)
    {
        _lookupsRepository = lookupsRepository;
        RuleFor(x => x.Code)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(20)
            .WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters long.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(250)
            .WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters long.");

        RuleFor(x => x.LookupType)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(100)
            .WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters long.");

        RuleFor(x => x)
            .MustAsync(UniqueLookup)
            .WithMessage("Lookup must be unique");
    }

    private async Task<bool> UniqueLookup(AddLookupCommand command, CancellationToken cancellationToken)
    {
        var isUnique = await _lookupsRepository.IsUniqueLookup(command.Code, command.LookupType, cancellationToken);
        return isUnique;
    }
}