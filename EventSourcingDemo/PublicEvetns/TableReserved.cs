using MediatR;

namespace EventSourcingDemo.PublicEvetns;

public record TableReserved(
        Guid ReservationId,
        int TableId,
        string Name,
        DateTime DateTime,
        int NrOfGuests
    ) : INotification;