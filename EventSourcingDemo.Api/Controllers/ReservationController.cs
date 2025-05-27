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
    /// <returns>ReservationId (Guid)</returns>
    /// <response code="200">Reservation created</response>
    /// <example>
    /// POST /api/Reservation/create-reservation
    /// {
    ///   "tableId": 3,
    ///   "name": "howest",
    ///   "dateTime": "2025-05-27T16:25:00.365Z",
    ///   "nrOfGuests": 6
    /// }
    /// </example>
    [HttpPost("create-reservation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Guid> CreateReservation([FromBody] CreateReservationCommand command, [FromServices] IMediator mediator)
    {
        return await mediator.Send(command);
    }

    /// <summary>
    /// Order drinks for an existing reservation
    /// </summary>
    /// <param name="command">Drink order details</param>
    /// <returns></returns>
    /// <response code="200">Order placed</response>
    /// <example>
    /// POST /api/Reservation/order-drinks
    /// {
    ///   "reservationId": "8574f669-1c33-439f-ab64-1875367aa57e",
    ///   "order": {
    ///     "productName": "Zeven Zonden Avaritia",
    ///     "productId": 5668,
    ///     "quantity": 2,
    ///     "price": 3.5,
    ///     "comment": ""
    ///   }
    /// }
    /// </example>
    [HttpPost("order-drinks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> OrderDrinks([FromBody] OrderDrinksCommand command, [FromServices] IMediator mediator)
    {
        await mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Mark an order as served
    /// </summary>
    /// <param name="command">Serve drinks command</param>
    /// <returns></returns>
    /// <response code="200">Order marked as served</response>
    /// <example>
    /// POST /api/Reservation/serve-drinks
    /// {
    ///   "orderId": "00000000-0000-0000-0000-000000000000",
    ///   "reservationId": "8574f669-1c33-439f-ab64-1875367aa57e"
    /// }
    /// </example>
    [HttpPost("serve-drinks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ServeDrinks([FromBody] ServeDrinksCommand command, [FromServices] IMediator mediator)
    {
        await mediator.Send(command);
        return Ok();
    }
}
