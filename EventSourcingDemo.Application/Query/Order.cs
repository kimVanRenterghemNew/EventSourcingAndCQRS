namespace EventSourcingDemo.Application.Query;

public record Order(
    Guid OrderId,
    Guid ReservationId,
    int TableId,
    string ProductName,
    int ProductId,
    int Quantity,
    string Comment,
    string Status // Status als string
);