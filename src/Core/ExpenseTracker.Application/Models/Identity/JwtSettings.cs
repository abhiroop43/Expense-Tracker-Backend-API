namespace ExpenseTracker.Application.Models.Identity;

public class JwtSettings
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public double DurationInMinutes { get; set; }
}