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
    /// <response code="200">Seeding successful</response>
    /// <example>
    /// POST /api/Table/seed
    /// </example>
    [HttpPost("seed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReservation([FromServices] MongoDbTablesRepository repository)
    {
        await repository.SeedAsync();
        return new  OkResult();
    }

    /// <summary>
    /// Get all tables in a date range
    /// </summary>
    /// <param name="start">Start date (ISO8601)</param>
    /// <param name="end">End date (ISO8601)</param>
    /// <returns>List of tables with reservations</returns>
    /// <response code="200">Returns all tables</response>
    /// <example>
    /// GET /api/Table/all?start=2025-05-26T00:00:00Z&end=2025-05-28T00:00:00Z
    /// </example>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Application.Query.Table>> GetAllTables([FromQuery] DateTime start, [FromQuery] DateTime end, [FromServices] IMediator mediator)
    {
        return await mediator.Send(new GetTables(new Between(start, end)));
    }
}
