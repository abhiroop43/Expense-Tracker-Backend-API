using System;
using ExpenseTracker.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace ExpenseTracker.Persistence;

public static class PersistenceServiceRegistrations
{
  public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("ExpenseTrackerConnection");
    var databaseName = configuration.GetSection("DatabaseName").Value;
    var mongoClient = new MongoClient(connectionString);

    services.AddDbContext<ExpenseDbContext>(options => options.UseMongoDB(mongoClient, databaseName!));

    return services;
  }
}
