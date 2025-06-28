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
}