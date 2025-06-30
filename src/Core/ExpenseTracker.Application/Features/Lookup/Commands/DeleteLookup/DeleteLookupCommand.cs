using MediatR;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Lookup.Commands.DeleteLookup;

public class DeleteLookupCommand : IRequest<ObjectId>
{
    public ObjectId Id { get; set; }
}