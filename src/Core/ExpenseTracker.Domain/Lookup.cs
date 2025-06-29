using ExpenseTracker.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace ExpenseTracker.Domain;

public class Lookup : BaseEntity
{
    public required string Code { get; set; }
    public required string Description { get; set; }
    public required string LookupType { get; set; }

    [BsonExtraElements] public Dictionary<string, object>? Metadata { get; set; }
}