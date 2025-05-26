using MediatR;
using EventSourcingDemo.Application.Projector;

namespace EventSourcingDemo.Application.Commands;

public record ServeDrinksCommand(
    Guid OrderId,
    Guid ReservationId
) : IRequest;

public class ServeDrinksCommandHandler : IRequestHandler<ServeDrinksCommand>
{
    private readonly TablesStore _tablesStore;
    private readonly OrderCollection _orderCollection;
    public ServeDrinksCommandHandler(TablesStore tablesStore, OrderCollection orderCollection)
    {
        _tablesStore = tablesStore;
        _orderCollection = orderCollection;
    }
    public async Task Handle(ServeDrinksCommand request, CancellationToken cancellationToken)
    {
        var table = await _tablesStore.Get(request.ReservationId);
        table.ServeDrinks(request.OrderId);
        await _tablesStore.SaveAsync(table);
    }
}
