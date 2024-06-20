namespace EventSourcingDemo;
public record DrinksOrdered(Order Order) : TableEvents;
