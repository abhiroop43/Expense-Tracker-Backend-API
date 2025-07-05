using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using ExpenseTracker.Domain.Common;

namespace ExpenseTracker.Domain;

public class Lookup : BaseEntity
{
    public required string Code { get; set; }
    public required string Description { get; set; }
    public required string LookupType { get; set; }
    public string? Metadata { get; set; }

    [NotMapped]
    public Dictionary<string, object>? MetadataDictionary
    {
        get => Metadata == null ? null : JsonSerializer.Deserialize<Dictionary<string, object>>(Metadata);
        set => Metadata = value == null ? null : JsonSerializer.Serialize(value);
    }
}