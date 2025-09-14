using AutoMapper;
using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace ExpenseTracker.Application.Features.Transaction.Commands.UpdateTransaction;

public class UpdateTransactionCommandHandler(
    ITransactionsRepository transactionsRepository,
    ILookupsRepository lookupsRepository,
    IWalletsRepository walletsRepository,
    IMapper mapper,
    IUserService userService,
    ILogger<UpdateTransactionCommandHandler> logger) : IRequestHandler<UpdateTransactionCommand, ObjectId>
{
    public async Task<ObjectId> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateTransactionCommandValidator(userService, walletsRepository, lookupsRepository);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count > 0)
        {
            logger.LogWarning("Validation errors were detected for {0}", nameof(request));
            throw new BadRequestException("Validation Errors", validationResult);
        }

        var transaction = await transactionsRepository.GetByIdAsync(request.Id, cancellationToken);

        if (transaction == null)
        {
            logger.LogWarning("Transaction with id {0} was not found", request.Id);
            throw new NotFoundException(nameof(transaction), request.Id);
        }

        mapper.Map(request, transaction);

        await transactionsRepository.UpdateAsync(transaction, cancellationToken);
        return transaction.Id;
    }
}