using ExpenseTracker.Application.Contracts.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseTracker.Api.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AllowedRolesAttribute(params string[] roles) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();

        if (authHeader == null || !authHeader.StartsWith("Bearer "))
        {
            context.Result = new ForbidResult();
            return;
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();

        var authService = httpContext.RequestServices.GetRequiredService<IAuthService>();
        var hasRole = roles.Any(role => authService.IsRoleInJwt(token, role));
        if (!hasRole)
        {
            context.Result = new ForbidResult();
            return;
        }

        await next();
    }
}