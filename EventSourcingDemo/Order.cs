namespace EventSourcingDemo;

public record Order(string ProductName, int ProductId, int Quantity, int Price, string Comment);
