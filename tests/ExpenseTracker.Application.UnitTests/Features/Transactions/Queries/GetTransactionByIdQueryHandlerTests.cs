using AutoMapper;
using ExpenseTracker.Application.Contracts.Identity;
using ExpenseTracker.Application.Contracts.Persistence;
using ExpenseTracker.Application.Exceptions;
using ExpenseTracker.Application.Features.Transaction.Queries.GetTransactionById;
using ExpenseTracker.Domain;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using Shouldly;

namespace ExpenseTracker.Application.UnitTests.Features.Transactions.Queries;

public class GetTransactionByIdQueryHandlerTests
{
    private readonly Mock<ILogger<GetTransactionByIdQueryHandler>> _logger = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<ITransactionsRepository> _repo = new();
    private readonly Mock<IUserService> _userService = new();

    private GetTransactionByIdQueryHandler CreateSuite()
    {
        return new GetTransactionByIdQueryHandler(_repo.Object, _mapper.Object, _userService.Object, _logger.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnDto_WhenTransactionExistsAndUserAuthorized()
    {
        // Arrange
        var id = ObjectId.GenerateNewId();
        var userId = Guid.NewGuid().ToString();
        var transaction = new Transaction
        {
            Id = id,
            CreatedBy = userId,
            TransactionTypeCode = "EXP",
            WalletId = ObjectId.GenerateNewId(),
            TransactionCategoryCode = "HLT"
        };
        var dto = new TransactionDetailDto { Id = id.ToString() };

        _repo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);
        _userService.Setup(u => u.UserId).Returns(userId);
        _mapper.Setup(m => m.Map<TransactionDetailDto>(transaction)).Returns(dto);

        var sut = CreateSuite();
        var query = new GetTransactionByIdQuery { Id = id };

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(id.ToString());
        _repo.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        _mapper.Verify(m => m.Map<TransactionDetailDto>(transaction), Times.Once);
        _logger.Verify(
            l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenTransactionDoesNotExist()
    {
        // Arrange
        var id = ObjectId.GenerateNewId();
        _repo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Transaction?)null);

        var sut = CreateSuite();
        var query = new GetTransactionByIdQuery { Id = id };

        // Act
        var ex = await Should.ThrowAsync<NotFoundException>(() => sut.Handle(query, CancellationToken.None));

        // Assert
        ex.Message.ShouldContain("was not found");
        _logger.Verify(
            l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) =>
                    v.ToString()!.Contains("was not found", StringComparison.OrdinalIgnoreCase)),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowForbiddenException_WhenUserNotOwner()
    {
        // Arrange
        var id = ObjectId.GenerateNewId();
        var ownerId = Guid.NewGuid().ToString();
        var otherUserId = Guid.NewGuid().ToString();
        var transaction = new Transaction
        {
            Id = id,
            CreatedBy = ownerId,
            TransactionTypeCode = "INC",
            WalletId = ObjectId.GenerateNewId(),
            TransactionCategoryCode = "SAL"
        };

        _repo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);
        _userService.Setup(u => u.UserId).Returns(otherUserId);

        var sut = CreateSuite();
        var query = new GetTransactionByIdQuery { Id = id };

        // Act
        var ex = await Should.ThrowAsync<ForbiddenException>(() => sut.Handle(query, CancellationToken.None));

        // Assert
        ex.Message.ShouldContain("not authorized");
        _logger.Verify(
            l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) =>
                    v.ToString()!.Contains("Unauthorized access", StringComparison.OrdinalIgnoreCase)),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}