using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
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

        if (lookups != null && lookups.Any()) throw new BadRequestException("Lookups data already present");

        List<Domain.Lookup> lookpsToBeAdded =
        [
            new()
            {
                Id = ObjectId.GenerateNewId(),
                LookupType = "TRANSACTIONTYPE",
                Code = "INC",
                Description = "Income"
            },
            new()
            {
                Id = ObjectId.GenerateNewId(),
                LookupType = "TRANSACTIONTYPE",
                Code = "EXP",
                Description = "Expense"
            }
        ];

        await lookupsRepository.AddSeedData(lookpsToBeAdded, cancellationToken);
    }
}