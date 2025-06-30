using MediatR;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Lookup.Commands.AddLookup;

public class AddLookupCommand : IRequest<ObjectId>
{
    public required string Code { get; set; }
    public required string Description { get; set; }
    public required string LookupType { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
}