using EventSourcingDemo.Application.Query;
using MediatR;

namespace EventSourcingDemo.Application.Projector;

public class TableProjector(TablesCollection tablesCollection) : INotificationHandler<PublicEvents.TableReserved>, INotificationHandler<PublicEvents.DrinksOrdered>
{
    public async Task Handle(PublicEvents.TableReserved notification, CancellationToken cancellationToken)
    {
        var table = await tablesCollection.GetAsync(notification.TableId);
        table = table.Add(new (
            notification.ReservationId,
            notification.Name,
            notification.DateTime,
            notification.NrOfGuests,
            0
        ));
        await tablesCollection.UpdateAsync(table);
    }

    public async Task Handle(PublicEvents.DrinksOrdered notification, CancellationToken cancellationToken)
    {
        var table = await tablesCollection.GetAsync(notification.TableId);

        var reservation = table.Reservations.First(r => r.ReservationId == notification.ReservationId) ;

        if (reservation != null)
        {
            reservation = reservation with { TotalCost = reservation.TotalCost + notification.Order.Price };
            table = table.Update(reservation);
        }

        await tablesCollection.UpdateAsync(table);
    }
}

