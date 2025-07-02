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
        CreateMap<Lookup, LookupDetailsDto>();
        CreateMap<AddLookupCommand, Lookup>();
        CreateMap<UpdateLookupCommand, Lookup>();
    }
}