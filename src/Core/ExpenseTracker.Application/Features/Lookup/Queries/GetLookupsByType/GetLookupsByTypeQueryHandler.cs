using AutoMapper;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;

namespace ExpenseTracker.Application.Features.Lookup.Queries.GetLookupsByType;

public class GetLookupsByTypeQueryHandler(IMapper mapper, ILookupsRepository lookupsRepository)
    : IRequestHandler<GetLookupsByTypeQuery, IReadOnlyList<LookupDto>>
{
    public async Task<IReadOnlyList<LookupDto>> Handle(GetLookupsByTypeQuery request,
        CancellationToken cancellationToken)
    {
        var lookups = await lookupsRepository.GetLookupsByType(request.LookupTypeCode, cancellationToken);

        if (!lookups.Any()) throw new BadRequestException("No lookups found for this type code");

        return mapper.Map<IReadOnlyList<LookupDto>>(lookups);
    }
}