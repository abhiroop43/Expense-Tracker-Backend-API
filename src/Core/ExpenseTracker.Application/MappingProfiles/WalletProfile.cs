using AutoMapper;
using ExpenseTracker.Application.Features.Wallet.Commands.AddWallet;
using ExpenseTracker.Domain;

namespace ExpenseTracker.Application.MappingProfiles;

public class WalletProfile : Profile
{
    public WalletProfile()
    {
        CreateMap<AddWalletCommand, Wallet>();
    }
}