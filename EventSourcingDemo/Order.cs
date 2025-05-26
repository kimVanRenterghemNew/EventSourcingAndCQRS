namespace EventSourcingDemo;

public record Order(string ProductName, int ProductId, int Quantity, double Price, string Comment);
