using MediatR;

namespace EventSourcingDemo.PublicEvents;

public record DrinksOrdered(Guid OrderId, Guid ReservationId, Order Order, int TableId, string Name) : INotification;