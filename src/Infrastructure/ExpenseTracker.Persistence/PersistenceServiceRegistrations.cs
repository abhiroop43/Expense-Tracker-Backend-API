using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Persistence.DatabaseContext;
using ExpenseTracker.Persistence.Repositories;
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

    services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    services.AddScoped<ILookupsRepository, LookupsRepository>();

    return services;
  }
}
