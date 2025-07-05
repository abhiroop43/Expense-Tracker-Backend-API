using System.Text.Json;
using AutoMapper;
using ExpenseTracker.Application.Features.Lookup.Commands.AddLookup;
using ExpenseTracker.Application.Features.Lookup.Commands.UpdateLookup;
using ExpenseTracker.Application.Features.Lookup.Queries.GetLookupById;
using ExpenseTracker.Application.Features.Lookup.Queries.GetLookupsByType;
using ExpenseTracker.Domain;

namespace ExpenseTracker.Application.MappingProfiles;

public class LookupProfile : Profile
{
    public LookupProfile()
    {
        CreateMap<Lookup, LookupDto>();

        CreateMap<Lookup, LookupDetailsDto>().ForMember(d => d.Metadata,
            opt => opt.MapFrom((src, _) =>
                src.Metadata == null
                    ? null
                    : JsonSerializer.Deserialize<Dictionary<string, object>>(src.Metadata.ToString())));

        CreateMap<AddLookupCommand, Lookup>()
            .ForMember(
                d => d.Metadata,
                opt => opt.MapFrom((src, _) =>
                    src.Metadata == null ? null : JsonSerializer.Serialize(src.Metadata)));

        CreateMap<UpdateLookupCommand, Lookup>();
    }
}