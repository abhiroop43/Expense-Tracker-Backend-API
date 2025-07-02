using MediatR;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Lookup.Commands.UpdateLookup;

public class UpdateLookupCommand : IRequest<ObjectId>
{
    public ObjectId Id { get; set; }
    public required string Description { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
}