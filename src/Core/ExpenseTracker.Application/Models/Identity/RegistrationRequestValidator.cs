using FluentValidation;

namespace ExpenseTracker.Application.Models.Identity;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
{
    public RegistrationRequestValidator()
    {
        const string missingRequiredFieldMessage = "{PropertyName} is required.";
        const string incorrectMaxLengthMessage = "{PropertyName} must not exceed {ComparisonValue} characters.";
        const string incorrectMinLengthMessage = "{PropertyName} must be at least {ComparisonValue} characters.";
        RuleFor(request => request.FirstName)
            .NotEmpty()
            .WithMessage(missingRequiredFieldMessage)
            .MaximumLength(50)
            .WithMessage(incorrectMaxLengthMessage)
            .MinimumLength(3)
            .WithMessage(incorrectMinLengthMessage);

        RuleFor(request => request.LastName)
            .NotEmpty()
            .WithMessage(missingRequiredFieldMessage)
            .MaximumLength(50)
            .WithMessage(incorrectMaxLengthMessage)
            .MinimumLength(3)
            .WithMessage(incorrectMinLengthMessage);

        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage(missingRequiredFieldMessage)
            .EmailAddress()
            .WithMessage("{PropertyName} must be a valid email address.");

        RuleFor(request => request.Username)
            .NotEmpty()
            .WithMessage(missingRequiredFieldMessage)
            .MaximumLength(20)
            .WithMessage(incorrectMaxLengthMessage)
            .MinimumLength(6)
            .WithMessage(incorrectMinLengthMessage);

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage(missingRequiredFieldMessage)
            .MinimumLength(8)
            .WithMessage(incorrectMinLengthMessage)
            .MaximumLength(15)
            .WithMessage(incorrectMaxLengthMessage)
            .Matches("[A-Z]").WithMessage("'{PropertyName}' must contain one or more capital letters.")
            .Matches("[a-z]").WithMessage("'{PropertyName}' must contain one or more lowercase letters.")
            .Matches(@"\d").WithMessage("'{PropertyName}' must contain one or more digits.")
            .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]")
            .WithMessage("'{ PropertyName}' must contain one or more special characters.")
            .Matches("^[^£# “”]*$")
            .WithMessage("'{PropertyName}' must not contain the following characters £ # “” or spaces.");
    }
}