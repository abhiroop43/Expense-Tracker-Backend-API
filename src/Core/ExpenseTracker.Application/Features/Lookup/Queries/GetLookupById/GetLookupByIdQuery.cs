using MediatR;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Lookup.Queries.GetLookupById;

public class GetLookupByIdQuery : IRequest<LookupDetailsDto>
{
    public ObjectId Id { get; set; }
}