using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DotNetEnv;
using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Exceptions;
using ExpenseTracker.Application.Models.Identity;
using ExpenseTracker.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace ExpenseTracker.Identity.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IOptions<JwtSettings> jwtSettings,
    IMapper mapper,
    ILogger<AuthService> logger) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<AuthResponse> LoginAsync(AuthRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            logger.LogWarning("User with email {0} does not exist", request.Email);
            throw new BadRequestException("Email and/or Password are incorrect");
        }

        // to check failure counts and lock account
        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            logger.LogWarning("User {0} entered incorrect password: {1}", request.Email, request.Password);
            throw new BadRequestException("Email and/or Password are incorrect");
        }

        var jwtSecurityToken = await GenerateToken(user);

        var response = new AuthResponse
        {
            Id = user.Id.ToString(),
            Email = user.Email!,
            Username = user.UserName!,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
        };

        return response;
    }

    public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
    {
        var user = mapper.Map<ApplicationUser>(request);
        user.EmailConfirmed = true;

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            logger.LogWarning("User {0} failed to register", request.Email);
            var sb = new StringBuilder();

            foreach (var error in result.Errors) sb.Append($"- {error.Description}\n");

            throw new BadRequestException($"{sb}");
        }

        await userManager.AddToRoleAsync(user, "User");
        return new RegistrationResponse { UserId = user.Id.ToString() };
    }

    private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
    {
        var userClaims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);

        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, ObjectId.GenerateNewId().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName!),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName!),
                new Claim("uid", user.Id.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

        Env.TraversePath().Load();
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.GetString("JWT_SECRET_KEY")));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);

        var jwtSecurityToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }
}