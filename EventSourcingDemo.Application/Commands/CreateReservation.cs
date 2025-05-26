using EventSourcingDemo.Application.Interfaces;
using MediatR;

namespace EventSourcingDemo.Application.Commands;

public record CreateReservationCommand(
    int TableId,
    string Name,
    DateTime DateTime,
    int NrOfGuests
) : IRequest<Guid>;

public class CreateReservationHandler(TablesStore tablesStore) : IRequestHandler<CreateReservationCommand, Guid>
{
    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var table = new Table(request.TableId, request.DateTime, request.Name, request.NrOfGuests);
        await tablesStore.SaveAsync(table);
        return table.ReservationId;
    }
}