namespace EventSourcingDemo.PublicEvetns;

internal record DrinksOrdered(Order Order, int TableId, string Name);
