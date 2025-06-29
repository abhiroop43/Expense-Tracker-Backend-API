using System;
using ExpenseTracker.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.Bson;

namespace ExpenseTracker.Persistence.Configurations;

public class LookupConfiguration : IEntityTypeConfiguration<Lookup>
{
    public void Configure(EntityTypeBuilder<Lookup> builder)
    {
        builder.HasData(
            new Lookup
            {
                Id = ObjectId.GenerateNewId(),
                LookupType = "TRANSACTIONTYPE",
                Code = "INC",
                Description = "Income"
            }
        );

        builder.Property(p => p.Code).HasMaxLength(20).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(250).IsRequired();
        builder.Property(p => p.LookupType).HasMaxLength(100).IsRequired();
    }
}