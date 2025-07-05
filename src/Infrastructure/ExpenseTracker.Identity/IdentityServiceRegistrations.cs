using AspNetCore.Identity.MongoDbCore.Models;
using ExpenseTracker.Identity.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;

namespace ExpenseTracker.Identity;

public static class IdentityServiceRegistrations
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ExpenseTrackerConnection");
        var databaseName = configuration.GetSection("DatabaseName").Value;

        services.AddIdentity<ApplicationUser, MongoIdentityRole<ObjectId>>()
            .AddMongoDbStores<ApplicationUser, MongoIdentityRole<ObjectId>, ObjectId>
            (
                connectionString, databaseName
            );

        return services;
    }
}