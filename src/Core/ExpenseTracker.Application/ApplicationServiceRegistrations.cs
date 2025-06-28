using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Application;

public static class ApplicationServiceRegistrations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}