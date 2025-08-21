using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DotNetEnv;
using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Exceptions;
using ExpenseTracker.Application.Models.Identity;
using ExpenseTracker.Identity.Models;
using FirebaseAdmin.Auth;
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

    public async Task<AuthResponse> LoginAsync(GoogleLoginRequest request)
    {
        var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(request.IdToken);

        // Check if user exists in your local identity system by Firebase UID or email
        var user = await FindOrCreateUserFromFirebaseToken(decodedToken);

        // Generate your own JWT for this user
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

    public bool IsRoleInJwt(string token, string role)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        return jwtToken.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Any(c => c.Value == role);
    }

    private async Task<ApplicationUser> FindOrCreateUserFromFirebaseToken(FirebaseToken firebaseToken)
    {
        var user = await userManager.FindByEmailAsync(firebaseToken.Claims["email"].ToString()!);

        if (user == null)
        {
            // User does not exist, create a new one based on Firebase info
            user = new ApplicationUser
            {
                Email = firebaseToken.Claims["email"].ToString(),
                UserName = firebaseToken.Claims["email"].ToString(), // Or a generated username
                FirstName = firebaseToken.Claims.TryGetValue("name", out var claim)
                    ? claim.ToString()
                    : "", // Use 'name' or 'displayName'
                LastName = "", // Firebase doesn't always provide separate first/last names directly
                EmailConfirmed = bool.Parse(firebaseToken.Claims["email_verified"].ToString()!),
                // You might want to store Firebase UID in your ApplicationUser model
                FirebaseUid = firebaseToken.Uid // Add a FirebaseUid property to ApplicationUser
            };

            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                var sb = new StringBuilder();
                foreach (var error in result.Errors) sb.Append($"- {error.Description}\n");
                throw new ServerException($"Failed to create user from Firebase token: {sb}");
            }

            await userManager.AddToRoleAsync(user, "User"); // Assign default role
        }
        else
        {
            // User exists, you might want to update their Firebase UID if it's new
            // or ensure consistency.
            if (!string.IsNullOrEmpty(user.FirebaseUid)) return user;
            user.FirebaseUid = firebaseToken.Uid;
            await userManager.UpdateAsync(user);
        }

        return user;
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