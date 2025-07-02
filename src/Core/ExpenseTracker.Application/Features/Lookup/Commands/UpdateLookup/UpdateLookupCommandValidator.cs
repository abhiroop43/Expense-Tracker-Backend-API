using FluentValidation;

namespace ExpenseTracker.Application.Features.Lookup.Commands.UpdateLookup;

public class UpdateLookupCommandValidator : AbstractValidator<UpdateLookupCommand>
{
    public UpdateLookupCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(250)
            .WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters long.");
    }
}