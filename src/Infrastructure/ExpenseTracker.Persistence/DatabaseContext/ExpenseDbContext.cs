using ExpenseTracker.Domain;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace ExpenseTracker.Persistence.DatabaseContext;

public class ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : DbContext(options)
{
  public DbSet<Lookup>? Lookups { get; set; }
  public DbSet<Wallet>? Wallets { get; set; }
  public DbSet<Transaction>? Transactions { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseDbContext).Assembly);
    modelBuilder.Entity<Lookup>().ToCollection("lookups");
    base.OnModelCreating(modelBuilder);
  }

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return base.SaveChangesAsync(cancellationToken);
  }
}
