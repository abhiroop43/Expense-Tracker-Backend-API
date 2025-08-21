using System.Security.Claims;
using System.Text.Encodings.Web;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Identity.Firebase;

public class FirebaseAuthenticationHandler(
    IOptionsMonitor<FirebaseAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<FirebaseAuthenticationOptions>(options, logger, encoder)
{
    private readonly ILogger<FirebaseAuthenticationHandler> _logger =
        logger.CreateLogger<FirebaseAuthenticationHandler>();

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization")) return AuthenticateResult.NoResult();

        string? authorizationHeader = Request.Headers["Authorization"];

        if (string.IsNullOrEmpty(authorizationHeader)) return AuthenticateResult.NoResult();

        if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return AuthenticateResult.NoResult();

        var idToken = authorizationHeader.Substring("Bearer ".Length).Trim();

        try
        {
            // Verify the Firebase ID token
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);

            // Create claims from the decoded token
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, decodedToken.Uid),
                new(ClaimTypes.Email,
                    decodedToken.Claims.TryGetValue("email", out var decodedTokenClaim)
                        ? decodedTokenClaim.ToString()!
                        : ""),
                new("uid", decodedToken.Uid) // Custom claim for your UserService
                // Add other claims as needed from decodedToken.Claims
            };

            // Add roles from Firebase custom claims if you're using them
            if (decodedToken.Claims.TryGetValue("roles", out var claim1) && claim1 is List<object> roles)
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.ToString()!)));

            // Firebase provides 'auth_time', 'firebase.sign_in_provider', etc. You can map these if relevant.
            // For example, to get the display name:
            if (decodedToken.Claims.TryGetValue("name", out var claim))
                claims.Add(new Claim(ClaimTypes.Name, claim.ToString()!));
            else if (decodedToken.Claims.TryGetValue("displayName",
                         out var tokenClaim)) // Sometimes display name is available
                claims.Add(new Claim(ClaimTypes.Name, tokenClaim.ToString()!));


            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (FirebaseAuthException ex)
        {
            _logger.LogError(ex, "Firebase ID Token verification failed: {ErrorMessage}", ex.Message);
            return AuthenticateResult.Fail($"Firebase ID Token verification failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during Firebase ID Token authentication.");
            return AuthenticateResult.Fail($"An unexpected error occurred: {ex.Message}");
        }
    }
}