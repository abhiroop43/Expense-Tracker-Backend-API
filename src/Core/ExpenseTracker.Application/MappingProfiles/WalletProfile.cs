using AutoMapper;
using ExpenseTracker.Application.Features.Wallet.Commands.AddWallet;
using ExpenseTracker.Application.Features.Wallet.Commands.UpdateWallet;
using ExpenseTracker.Application.Features.Wallet.Queries.GetAllWallets;
using ExpenseTracker.Application.Features.Wallet.Queries.GetWalletById;
using ExpenseTracker.Domain;

namespace ExpenseTracker.Application.MappingProfiles;

public class WalletProfile : Profile
{
    public WalletProfile()
    {
        CreateMap<AddWalletCommand, Wallet>();
        CreateMap<UpdateWalletCommand, Wallet>();

        CreateMap<Wallet, WalletDto>();
        CreateMap<Wallet, WalletDetailDto>();
    }
}