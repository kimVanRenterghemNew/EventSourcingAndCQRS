using MediatR;

namespace EventSourcingDemo.PublicEvetns;

public record DrinksOrdered(Guid ReservationId, Order Order, int TableId, string Name) : INotification;
