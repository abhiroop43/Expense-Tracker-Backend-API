using MediatR;

namespace ExpenseTracker.Application.Features.Lookup.Queries.GetLookupsByType;

public class GetLookupsByTypeQuery : IRequest<IReadOnlyList<LookupDto>>
{
    public required string LookupTypeCode { get; set; }
}