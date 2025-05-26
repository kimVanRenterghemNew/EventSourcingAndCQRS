namespace EventSourcingDemo;

public record Order(Guid OrderId, string ProductName, int ProductId, int Quantity, double Price, string Comment);
