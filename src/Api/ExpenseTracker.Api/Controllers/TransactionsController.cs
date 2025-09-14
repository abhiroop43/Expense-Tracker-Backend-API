using ExpenseTracker.Application.Features.Transaction.Commands.AddTransaction;
using ExpenseTracker.Application.Features.Transaction.Commands.DeleteTransaction;
using ExpenseTracker.Application.Features.Transaction.Commands.UpdateTransaction;
using ExpenseTracker.Application.Features.Transaction.Commands.UploadReceipt;
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

    [HttpPost("uploadReceipt")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadReceipt(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var command = new UploadReceiptCommand
        {
            File = stream,
            FileName = file.FileName
        };

        var imageUrl = await mediator.Send(command);
        return Created(nameof(UploadReceipt), new { ImageUrl = imageUrl });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> AddTransaction([FromBody] AddTransactionCommand command)
    {
        var transactionId = await mediator.Send(command);
        return Created(nameof(GetTransactionById), new { Id = transactionId });
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateTransaction([FromBody] UpdateTransactionCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTransaction([FromRoute] string id)
    {
        await mediator.Send(new DeleteTransactionCommand { Id = ObjectId.Parse(id) });
        return NoContent();
    }
}