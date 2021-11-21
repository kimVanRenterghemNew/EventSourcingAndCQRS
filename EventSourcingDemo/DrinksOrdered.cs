namespace EventSourcingDemo
{
    public class DrinksOrdered : ITableEvents
    {
        public Order Order { get; init; }
    }
}