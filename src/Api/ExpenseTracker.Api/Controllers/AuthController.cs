using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Models.Identity;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        return Ok(await authService.LoginAsync(request));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
    {
        return Ok(await authService.RegisterAsync(request));
    }

    [HttpPost("login-google")]
    public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleLoginRequest request)
    {
        // This is a simplified example. You'd likely want to handle user creation/linking more robustly.
        try
        {
            return Ok(await authService.LoginAsync(request));
        }
        catch (FirebaseAuthException ex)
        {
            return Unauthorized(new { message = $"Firebase token verification failed: {ex.Message}" });
        }
    }
}