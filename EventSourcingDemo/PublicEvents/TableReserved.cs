using MediatR;

namespace EventSourcingDemo.PublicEvents;

public record TableReserved(
        Guid ReservationId,
        int TableId,
        string Name,
        DateTime DateTime,
        int NrOfGuests
    ) : INotification;