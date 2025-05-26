using EventSourcingDemo.Application.Interfaces;
using MediatR;

namespace EventSourcingDemo.Application.Commands;

public record OrderDrinksCommand(
    Guid ReservationId,
    Order Order
) : IRequest;

public class OrderDrinksCommandHandler(TablesStore tablesStore) : IRequestHandler<OrderDrinksCommand>
{
    public async Task Handle(OrderDrinksCommand request, CancellationToken cancellationToken)
    {
        var table = await tablesStore.Get(request.ReservationId);

        table.OrderDrinks(request.Order);

        await tablesStore.SaveAsync(table);
    }
}