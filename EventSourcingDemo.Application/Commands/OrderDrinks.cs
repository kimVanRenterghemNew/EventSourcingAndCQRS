using MediatR;

namespace EventSourcingDemo.Application.Commands;

public record OrderDrinksCommand(
    Guid ReservationId,
    Order Order
) : IRequest;

public class OrderDrinksCommandHandler : IRequestHandler<OrderDrinksCommand>
{
    private readonly TablesStore _tablesStore;
    public OrderDrinksCommandHandler(TablesStore tablesStore)
    {
        _tablesStore = tablesStore;
    }
    public async Task Handle(OrderDrinksCommand request, CancellationToken cancellationToken)
    {
        var table = await _tablesStore.Get(request.ReservationId);

        table.OrderDrinks(request.Order);

        await _tablesStore.SaveAsync(table);
    }
}