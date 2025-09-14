using AutoMapper;
using ExpenseTracker.Application.Features.Transaction.Commands.AddTransaction;
using ExpenseTracker.Application.Features.Transaction.Commands.UpdateTransaction;
using ExpenseTracker.Application.Features.Transaction.Queries.GetAllTransactions;
using ExpenseTracker.Application.Features.Transaction.Queries.GetTransactionById;
using ExpenseTracker.Domain;
using MongoDB.Bson;

namespace ExpenseTracker.Application.MappingProfiles;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Transaction, TransactionListDto>();
        CreateMap<Transaction, TransactionDetailDto>();
        CreateMap<AddTransactionCommand, Transaction>()
            .ForMember(dest => dest.WalletId, opt => opt.MapFrom(src => ObjectId.Parse(src.WalletId)));
        CreateMap<UpdateTransactionCommand, Transaction>();
    }
}