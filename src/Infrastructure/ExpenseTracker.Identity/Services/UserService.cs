using System.Security.Claims;
using AutoMapper;
using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Models.Identity;
using ExpenseTracker.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Identity.Services;

public class UserService(
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor contextAccessor,
    IMapper mapper,
    ILogger<UserService> logger) : IUserService
{
    public string? UserId => contextAccessor.HttpContext?.User?.FindFirstValue("uid");

    public async Task<ApplicationUserDto?> GetUserDetailsAsync(string id, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user == null)
        {
            logger.LogWarning("No user found with id: {0}", id);
            return null;
        }

        return mapper.Map<ApplicationUserDto>(user);
    }
}