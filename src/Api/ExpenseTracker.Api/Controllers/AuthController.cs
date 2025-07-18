using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Models.Identity;
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
}