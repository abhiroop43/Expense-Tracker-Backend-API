using ExpenseTracker.Application.Features.Wallet.Commands.AddWallet;
using ExpenseTracker.Application.Features.Wallet.Commands.DeleteWallet;
using ExpenseTracker.Application.Features.Wallet.Commands.UpdateWallet;
using ExpenseTracker.Application.Features.Wallet.Commands.UploadWalletLogo;
using ExpenseTracker.Application.Features.Wallet.Queries.GetAllWallets;
using ExpenseTracker.Application.Features.Wallet.Queries.GetWalletById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WalletsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllWallets()
    {
        var wallets = await mediator.Send(new GetAllWalletsQuery());
        return Ok(wallets);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWalletById([FromRoute] string id)
    {
        var wallet = await mediator.Send(new GetWalletByIdQuery { Id = ObjectId.Parse(id) });
        return Ok(wallet);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateWallet([FromBody] AddWalletCommand command)
    {
        var walletId = await mediator.Send(command);
        return Created(nameof(GetWalletById), new { Id = walletId.ToString() });
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateWallet([FromBody] UpdateWalletCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteWallet([FromRoute] string id)
    {
        await mediator.Send(new DeleteWalletCommand { Id = ObjectId.Parse(id) });
        return NoContent();
    }

    [HttpPost("uploadImage")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var command = new UploadWalletLogoCommand
        {
            File = stream,
            FileName = file.FileName
        };

        var imageUrl = await mediator.Send(command);
        return Created(nameof(UploadImage), new { ImageUrl = imageUrl });
    }
}