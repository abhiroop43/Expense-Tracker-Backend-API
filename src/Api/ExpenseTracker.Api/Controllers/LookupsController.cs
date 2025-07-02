using ExpenseTracker.Application.Features.Lookup.Commands.AddLookup;
using ExpenseTracker.Application.Features.Lookup.Commands.DeleteLookup;
using ExpenseTracker.Application.Features.Lookup.Commands.SeedLookups;
using ExpenseTracker.Application.Features.Lookup.Commands.UpdateLookup;
using ExpenseTracker.Application.Features.Lookup.Queries.GetLookupById;
using ExpenseTracker.Application.Features.Lookup.Queries.GetLookupsByType;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LookupsController(IMediator mediator) : ControllerBase
{
    [HttpGet("getByCode/{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByLookupTypeCode(string code)
    {
        var lookups = await mediator.Send(new GetLookupsByTypeQuery { LookupTypeCode = code });
        return Ok(lookups);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLookupById([FromRoute] string id)
    {
        var lookup = await mediator.Send(new GetLookupByIdQuery { Id = ObjectId.Parse(id) });
        return Ok(lookup);
    }

    // should be restricted only to Admin roles
    [HttpPost("seed")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SeedLookups()
    {
        await mediator.Send(new SeedLookupsCommand());
        return Created(nameof(GetByLookupTypeCode), null);
    }

    // should be restricted only to Admin roles
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddNewLookup([FromBody] AddLookupCommand command)
    {
        await mediator.Send(command);
        return Created(nameof(GetByLookupTypeCode), null);
    }

    // should be restricted only to Admin roles
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLookup([FromBody] UpdateLookupCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    // should be restricted only to Admin roles
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLookup([FromRoute] string id)
    {
        await mediator.Send(new DeleteLookupCommand { Id = ObjectId.Parse(id) });
        return NoContent();
    }
}