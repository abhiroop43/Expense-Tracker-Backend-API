using ExpenseTracker.Application.Models.Identity;

namespace ExpenseTracker.Application.Contracts.Identity;

public interface IUserService
{
    public string? UserId { get; }
    Task<ApplicationUserDto?> GetUserDetailsAsync(string id, CancellationToken cancellationToken = default);
}