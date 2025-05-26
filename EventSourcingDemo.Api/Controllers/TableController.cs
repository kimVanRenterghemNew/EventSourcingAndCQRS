using EventSourcingDemo.Application.Query;
using EventSourcingDemo.MongoDb;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcingDemo.Api.Controllers;
[ApiController]
[Route("api/Table")]
public class TableController
{
    /// <summary>
    /// Create a new reservation
    /// </summary>
    /// <param name="command">Reservation details</param>
    /// <returns></returns>
    [HttpPost("seed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReservation([FromServices] MongoDbTablesRepository repository)
    {
        await repository.SeedAsync();
        return new  OkResult();
    }





    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Application.Query.Table>> GetAllTables([FromQuery] DateTime start, [FromQuery] DateTime end, [FromServices] IMediator mediator)
    {
        return await mediator.Send(new GetTables(new Between(start, end)));
    }
}
