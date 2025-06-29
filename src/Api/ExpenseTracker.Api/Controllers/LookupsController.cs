using ExpenseTracker.Application.Features.Lookup.Commands.SeedLookups;
using ExpenseTracker.Application.Features.Lookup.Queries.GetLookupsByType;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

    // should be restricted only to Admin roles
    [HttpPost("seed")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SeedLookups()
    {
        await mediator.Send(new SeedLookupsCommand());
        return Created(nameof(GetByLookupTypeCode), null);
    }
}