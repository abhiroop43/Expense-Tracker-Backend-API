using ExpenseTracker.Application.Models.Identity;

namespace ExpenseTracker.Application.Contracts.Identity;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(AuthRequest request);
    Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
    bool IsRoleInJwt(string token, string role);
}