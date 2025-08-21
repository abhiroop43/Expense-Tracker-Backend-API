using AutoMapper;
using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Features.Transaction.Queries.GetAllTransactions;
using ExpenseTracker.Domain;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using Shouldly;

namespace ExpenseTracker.Application.UnitTests.Features.Transactions.Queries;

public class GetAllTransactionsQueryHandlerTests
{
    private readonly Mock<ILogger<GetAllTransactionsQueryHandler>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<ITransactionsRepository> _repo = new();
    private readonly Mock<IUserService> _userService = new();

    private GetAllTransactionsQueryHandler CreateSut()
    {
        return new GetAllTransactionsQueryHandler(_repo.Object, _mapper.Object, _userService.Object, _logger.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedList_WhenTransactionsExist()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var walletId = ObjectId.GenerateNewId();
        var transactions = new List<Transaction>
        {
            new()
            {
                Id = ObjectId.GenerateNewId(),
                CreatedBy = userId,
                TransactionTypeCode = "INC",
                WalletId = walletId,
                TransactionCategoryCode = "SAL"
            },
            new()
            {
                Id = ObjectId.GenerateNewId(),
                CreatedBy = userId,
                TransactionTypeCode = "EXP",
                WalletId = walletId,
                TransactionCategoryCode = "CLT"
            }
        };
        var mapped = new List<TransactionListDto>
        {
            new() { Id = transactions[0].Id.ToString() },
            new() { Id = transactions[1].Id.ToString() }
        }.AsReadOnly();

        _userService.Setup(u => u.UserId).Returns(userId);
        _repo.Setup(r => r.GetTransactionsForUser(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactions);
        _mapper.Setup(m => m.Map<IReadOnlyList<TransactionListDto>>(transactions))
            .Returns(mapped);

        var sut = CreateSut();
        var query = new GetAllTransactionsQuery();

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.ShouldBe(mapped);
        _repo.Verify(r => r.GetTransactionsForUser(userId, It.IsAny<CancellationToken>()), Times.Once);
        _mapper.Verify(m => m.Map<IReadOnlyList<TransactionListDto>>(transactions), Times.Once);
        _logger.Verify(
            l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyListAndLogWarning_WhenNoTransactions()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var transactions = new List<Transaction>(); // empty
        var mapped = Array.Empty<TransactionListDto>() as IReadOnlyList<TransactionListDto>;

        _userService.Setup(u => u.UserId).Returns(userId);
        _repo.Setup(r => r.GetTransactionsForUser(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactions);
        _mapper.Setup(m => m.Map<IReadOnlyList<TransactionListDto>>(transactions))
            .Returns(mapped);

        var sut = CreateSut();
        var query = new GetAllTransactionsQuery();

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(0);
        _repo.Verify(r => r.GetTransactionsForUser(userId, It.IsAny<CancellationToken>()), Times.Once);
        _mapper.Verify(m => m.Map<IReadOnlyList<TransactionListDto>>(transactions), Times.Once);
        _logger.Verify(
            l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) =>
                    v.ToString()!.Contains("No transactions found", StringComparison.OrdinalIgnoreCase)),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}