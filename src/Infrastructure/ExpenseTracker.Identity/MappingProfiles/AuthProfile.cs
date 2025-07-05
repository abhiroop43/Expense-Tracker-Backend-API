using AutoMapper;
using ExpenseTracker.Application.Models.Identity;
using ExpenseTracker.Identity.Models;

namespace ExpenseTracker.Identity.MappingProfiles;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegistrationRequest, ApplicationUser>();
    }
}