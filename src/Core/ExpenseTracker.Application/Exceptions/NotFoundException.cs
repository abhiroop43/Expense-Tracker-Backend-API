namespace ExpenseTracker.Application.Exceptions;

public class NotFoundException(string name, object key) : Exception($"{name} with ({key}) was not found");