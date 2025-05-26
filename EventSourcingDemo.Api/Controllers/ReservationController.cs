using Microsoft.AspNetCore.Mvc;
using MediatR;
using EventSourcingDemo.Application.Commands;

namespace EventSourcingDemo.Api.Controllers;

[ApiController]
[Route("api/Reservation")]
public class ReservationController : ControllerBase
{

    /// <summary>
    /// Create a new reservation
    /// </summary>
    /// <param name="command">Reservation details</param>
    /// <returns></returns>
    [HttpPost("create-reservation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Guid> CreateReservation([FromBody] CreateReservationCommand command, IMediator mediator)
    {
        return await mediator.Send(command);
    }

    /// <summary>
    /// Order drinks for an existing reservation
    /// </summary>
    /// <param name="command">Drink order details</param>
    /// <returns></returns>
    [HttpPost("order-drinks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> OrderDrinks([FromBody] OrderDrinksCommand command, [FromServices] IMediator mediator)
    {
        await mediator.Send(command);
        return Ok();
    }
}
