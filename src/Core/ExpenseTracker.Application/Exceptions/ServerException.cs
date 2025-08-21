namespace ExpenseTracker.Application.Exceptions;

public class ServerException(string message) : Exception(message);