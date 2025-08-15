using AutoMapper;
using ExpenseTracker.Application.Features.Transaction.Queries.GetAllTransactions;
using ExpenseTracker.Application.Features.Transaction.Queries.GetTransactionById;
using ExpenseTracker.Domain;

namespace ExpenseTracker.Application.MappingProfiles;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Transaction, TransactionListDto>();
        CreateMap<Transaction, TransactionDetailDto>();
    }
}