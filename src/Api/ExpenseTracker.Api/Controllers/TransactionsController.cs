using ExpenseTracker.Application.Features.Transaction.Queries.GetAllTransactions;
using ExpenseTracker.Application.Features.Transaction.Queries.GetTransactionById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllTransaction()
    {
        var transactions = await mediator.Send(new GetAllTransactionsQuery());
        return Ok(transactions);
    }

    [HttpGet("id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransactionById([FromRoute] string id)
    {
        var transaction = await mediator.Send(new GetTransactionByIdQuery { Id = ObjectId.Parse(id) });
        return Ok(transaction);
    }
}