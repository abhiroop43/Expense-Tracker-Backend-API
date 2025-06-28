using FluentValidation.Results;

namespace ExpenseTracker.Application.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string message, ValidationResult validationResultErrors) : base(message)
    {
        ValidationResultErrors = validationResultErrors.ToDictionary();
    }

    public IDictionary<string, string[]>? ValidationResultErrors { get; set; }
}