using EventSourcingDemo.Application.Interfaces;
using MediatR;

namespace EventSourcingDemo.Application.Commands;

public record ServeDrinksCommand(
    Guid OrderId,
    Guid ReservationId
) : IRequest;

public class ServeDrinksCommandHandler(TablesStore tablesStore) : IRequestHandler<ServeDrinksCommand>
{
    public async Task Handle(ServeDrinksCommand request, CancellationToken cancellationToken)
    {
        var table = await tablesStore.Get(request.ReservationId);
        table.ServeDrinks(request.OrderId);
        await tablesStore.SaveAsync(table);
    }
}
