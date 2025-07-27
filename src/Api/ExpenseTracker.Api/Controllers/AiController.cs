using ExpenseTracker.Application.Contracts.AI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AiController(IAiRequestor aiRequestor) : ControllerBase
{
    [HttpGet("suggestion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetSuggestionsForUserAsync(CancellationToken cancellation)
    {
        // var results = await aiRequestor.GetSuggestionsForSavingsAsync(new List<UserTransaction>(), cancellation);

        return Ok();
    }
}