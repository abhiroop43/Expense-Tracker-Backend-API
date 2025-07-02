using AutoMapper;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Lookup.Queries.GetLookupById;

public class GetLookupByIdQueryHandler(
    IMapper mapper,
    ILookupsRepository lookupsRepository,
    ILogger<GetLookupByIdQueryHandler> logger) : IRequestHandler<GetLookupByIdQuery, LookupDetailsDto>
{
    public async Task<LookupDetailsDto> Handle(GetLookupByIdQuery request, CancellationToken cancellationToken)
    {
        var lookup = await lookupsRepository.GetByIdAsync(request.Id, cancellationToken);

        if (lookup == null)
        {
            logger.LogWarning("{0} with ID {1} not found", nameof(lookup), request.Id);
            throw new NotFoundException(nameof(lookup), request.Id);
        }

        return mapper.Map<LookupDetailsDto>(lookup);
    }
}