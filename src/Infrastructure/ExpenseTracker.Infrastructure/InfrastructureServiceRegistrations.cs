using ExpenseTracker.Application.Contracts.AI;
using ExpenseTracker.Infrastructure.AIService;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Infrastructure;

public static class InfrastructureServiceRegistrations
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IAIRequestor, AIRequestor>();
        return services;
    }
}