using AutoMapper;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Lookup.Commands.AddLookup;

public class AddLookupCommandHandler(
    IMapper mapper,
    ILookupsRepository lookupsRepository,
    ILogger<AddLookupCommandHandler> logger) : IRequestHandler<AddLookupCommand, ObjectId>
{
    public async Task<ObjectId> Handle(AddLookupCommand request, CancellationToken cancellationToken)
    {
        var validator = new AddLookupCommandValidator(lookupsRepository);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
        {
            logger.LogWarning("Validation errors detected in Add Lookup for {0}", nameof(request));
            throw new BadRequestException("Validation errors", validationResult);
        }

        var lookup = mapper.Map<Domain.Lookup>(request);

        await lookupsRepository.CreateAsync(lookup, cancellationToken);

        return lookup.Id;
    }
}