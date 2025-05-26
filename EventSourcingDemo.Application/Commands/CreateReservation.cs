using MediatR;

namespace EventSourcingDemo.Application.Commands;

public record CreateReservationCommand(
    int TableId,
    string Name,
    DateTime DateTime,
    int NrOfGuests
) : IRequest<Guid>;

public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, Guid>
{
    private readonly TablesStore _tablesStore;
    public CreateReservationHandler(TablesStore tablesStore)
    {
        _tablesStore = tablesStore;
    }
    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var table = new Table(request.TableId, request.DateTime, request.Name, request.NrOfGuests);
        await _tablesStore.SaveAsync(table);
        return table.ReservationId;
    }
}