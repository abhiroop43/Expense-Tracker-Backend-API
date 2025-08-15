using AutoMapper;
using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Transaction.Commands.AddTransaction;

public class AddTransactionCommandHandler(
    ITransactionsRepository transactionsRepository,
    ILookupsRepository lookupsRepository,
    IWalletsRepository walletsRepository,
    IMapper mapper,
    IUserService userService,
    ILogger<AddTransactionCommandHandler> logger) : IRequestHandler<AddTransactionCommand, ObjectId>
{
    public async Task<ObjectId> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
    {
        var validator = new AddTransactionCommandValidator(lookupsRepository, walletsRepository, userService);
        var result = await validator.ValidateAsync(request, cancellationToken);

        if (result.Errors.Count > 0)
        {
            logger.LogWarning("Validation errors were detected for {0}", nameof(request));
            throw new BadRequestException("Validation Errors", result);
        }

        var transaction = mapper.Map<Domain.Transaction>(request);
        await transactionsRepository.CreateAsync(transaction, cancellationToken);

        return transaction.Id;
    }
}