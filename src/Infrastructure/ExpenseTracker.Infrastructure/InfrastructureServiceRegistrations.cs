using ExpenseTracker.Application.Contracts.AI;
using ExpenseTracker.Application.Contracts.Storage;
using ExpenseTracker.Infrastructure.AIService;
using ExpenseTracker.Infrastructure.Configuration;
using ExpenseTracker.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Infrastructure;

public static class InfrastructureServiceRegistrations
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IAiRequestor, AiRequestor>();
        services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));
        services.AddScoped<IBlobStorageService, BlobStorageService>();
        return services;
    }
}