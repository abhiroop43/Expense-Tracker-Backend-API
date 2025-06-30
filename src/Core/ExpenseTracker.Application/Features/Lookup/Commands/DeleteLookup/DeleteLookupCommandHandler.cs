using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Lookup.Commands.DeleteLookup;

public class DeleteLookupCommandHandler(ILookupsRepository lookupsRepository, ILogger<DeleteLookupCommand> logger)
    : IRequestHandler<DeleteLookupCommand, ObjectId>
{
    public async Task<ObjectId> Handle(DeleteLookupCommand request, CancellationToken cancellationToken)
    {
        var lookup = await lookupsRepository.GetByIdAsync(request.Id, cancellationToken);

        if (lookup == null)
        {
            logger.LogError("Unable to find lookup with id: {0}", request.Id);
            throw new NotFoundException(nameof(lookup), request.Id);
        }

        await lookupsRepository.DeleteAsync(lookup, cancellationToken);

        return lookup.Id;
    }
}