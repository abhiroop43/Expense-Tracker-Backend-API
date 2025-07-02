namespace ExpenseTracker.Application.Features.Lookup.Queries.GetLookupById;

public class LookupDetailsDto
{
    public required string Id { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }
    public required string LookupType { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
}