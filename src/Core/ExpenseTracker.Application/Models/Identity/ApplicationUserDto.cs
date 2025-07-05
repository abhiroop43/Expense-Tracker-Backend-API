namespace ExpenseTracker.Application.Models.Identity;

public class ApplicationUserDto
{
    public required string Id { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}