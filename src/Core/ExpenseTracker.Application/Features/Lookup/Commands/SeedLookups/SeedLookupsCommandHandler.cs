using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using ExpenseTracker.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Lookup.Commands.SeedLookups;

public class SeedLookupsCommandHandler(ILookupsRepository lookupsRepository, ILogger<SeedLookupsCommandHandler> logger)
    : IRequestHandler<SeedLookupsCommand>
{
    public async Task Handle(SeedLookupsCommand request, CancellationToken cancellationToken)
    {
        var lookups = await lookupsRepository.GetAllAsync(cancellationToken);

        if (lookups != null && lookups.Any())
        {
            logger.LogWarning("Database has already been seeded. Lookups collection non-empty.");
            throw new BadRequestException("Lookups data already present");
        }

        List<Domain.Lookup> lookupsToBeAdded =
        [
            new()
            {
                Id = ObjectId.GenerateNewId(),
                LookupType = Constants.TransactionType,
                Code = "INC",
                Description = "Income"
            },
            new()
            {
                Id = ObjectId.GenerateNewId(),
                LookupType = Constants.TransactionType,
                Code = "EXP",
                Description = "Expense"
            },
            new()
            {
                Id = ObjectId.GenerateNewId(),
                LookupType = Constants.TransactionCategory,
                Code = "HLT",
                Description = "Health"
            },
            new()
            {
                Id = ObjectId.GenerateNewId(),
                LookupType = Constants.TransactionCategory,
                Code = "CLT",
                Description = "Clothing"
            },
            new()
            {
                Id = ObjectId.GenerateNewId(),
                LookupType = Constants.TransactionCategory,
                Code = "SAL",
                Description = "Salary"
            },
            new()
            {
                Id = ObjectId.GenerateNewId(),
                LookupType = Constants.TransactionCategory,
                Code = "SAV",
                Description = "Savings"
            },
            new()
            {
                Id = ObjectId.GenerateNewId(),
                LookupType = Constants.TransactionCategory,
                Code = "DIN",
                Description = "Dining"
            },
            new()
            {
                Id = ObjectId.GenerateNewId(),
                LookupType = Constants.TransactionCategory,
                Code = "PRL",
                Description = "Personal"
            },
            new()
            {
                Id = ObjectId.GenerateNewId(),
                LookupType = Constants.TransactionCategory,
                Code = "INS",
                Description = "Insurance"
            },
            new()
            {
                Id = ObjectId.GenerateNewId(),
                LookupType = Constants.TransactionCategory,
                Code = "OTH",
                Description = "Others"
            }
        ];

        await lookupsRepository.AddSeedData(lookupsToBeAdded, cancellationToken);
    }
}