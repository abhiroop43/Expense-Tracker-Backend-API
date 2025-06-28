using AutoMapper;
using ExpenseTracker.Application.Features.Lookup.Queries.GetLookupsByType;
using ExpenseTracker.Domain;

namespace ExpenseTracker.Application.MappingProfiles;

public class LookupProfile : Profile
{
    public LookupProfile()
    {
        CreateMap<Lookup, LookupDto>()
            // .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            ;
    }
}