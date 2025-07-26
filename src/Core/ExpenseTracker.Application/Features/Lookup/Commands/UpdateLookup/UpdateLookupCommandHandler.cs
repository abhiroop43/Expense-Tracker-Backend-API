using AutoMapper;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Lookup.Commands.UpdateLookup;

public class UpdateLookupCommandHandler(
    IMapper mapper,
    ILookupsRepository lookupsRepository,
    ILogger<UpdateLookupCommandHandler> logger) : IRequestHandler<UpdateLookupCommand, ObjectId>
{
    public async Task<ObjectId> Handle(UpdateLookupCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateLookupCommandValidator();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count > 0)
        {
            logger.LogWarning("Validation errors detected for {0}", nameof(request));
            throw new BadRequestException("Validation errors", validationResult);
        }

        var lookup = await lookupsRepository.GetByIdAsync(request.Id, cancellationToken);

        if (lookup == null)
        {
            logger.LogWarning("{0} with {1} was not found", nameof(lookup), request.Id);
            throw new NotFoundException(nameof(lookup), request.Id);
        }

        mapper.Map(request, lookup);

        var updatedLookup = await lookupsRepository.UpdateAsync(lookup, cancellationToken);
        return updatedLookup.Id;
    }
}