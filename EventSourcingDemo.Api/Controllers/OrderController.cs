using EventSourcingDemo.Application.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcingDemo.Api.Controllers;

[ApiController]
[Route("api/Order")]
public class OrderController : ControllerBase
{
    /// <summary>
    /// Get all orders
    /// </summary>
    /// <returns>List of all orders</returns>
    /// <response code="200">Returns all orders</response>
    /// <example>
    /// GET /api/Order
    /// </example>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Application.Query.Order>> GetAllOrders([FromServices] IMediator mediator)
    {
        return await mediator.Send(new GetOrders());
    }
}
