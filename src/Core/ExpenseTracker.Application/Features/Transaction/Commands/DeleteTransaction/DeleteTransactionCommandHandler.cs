using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Transaction.Commands.DeleteTransaction;

public class DeleteTransactionCommandHandler(
    ITransactionsRepository repository,
    ILogger<DeleteTransactionCommandHandler> logger) : IRequestHandler<DeleteTransactionCommand, Unit>
{
    public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (transaction == null)
        {
            logger.LogWarning("Transaction with id {0} was not found", request.Id);
            throw new NotFoundException(nameof(transaction), request.Id);
        }

        await repository.DeleteAsync(transaction, cancellationToken);
        return Unit.Value;
    }
}