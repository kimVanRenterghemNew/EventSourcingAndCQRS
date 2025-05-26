using MediatR;

namespace EventSourcingDemo.PublicEvents;

public record DrinksServed(Guid ReservationId, Guid OrderId, int TableId) : INotification;