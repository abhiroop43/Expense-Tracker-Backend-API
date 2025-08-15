using AutoMapper;
using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Application.Features.Transaction.Queries.GetTransactionById;

public class GetTransactionByIdQueryHandler(
    ITransactionsRepository repository,
    IMapper mapper,
    IUserService userService,
    ILogger<GetTransactionByIdQueryHandler> logger) : IRequestHandler<GetTransactionByIdQuery, TransactionDetailDto>
{
    public async Task<TransactionDetailDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (transaction == null)
        {
            logger.LogWarning("{0} with ID {1} was not found", nameof(transaction), request.Id);
            throw new NotFoundException(nameof(transaction), request.Id);
        }

        if (transaction.CreatedBy != userService.UserId)
        {
            logger.LogWarning("Unauthorized access to transaction with ID {0} by user {1}", transaction.Id,
                userService.UserId);
            throw new ForbiddenException("You are not authorized to access this resource");
        }

        return mapper.Map<TransactionDetailDto>(transaction);
    }
}