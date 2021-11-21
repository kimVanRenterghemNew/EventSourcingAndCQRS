namespace EventSourcingDemo.PublicEvents
{
    internal class DrinksOrdered
    {
        public Order Order { get; set; }
        public int TableId { get; init; }
        public string Name { get; init; }
    }
}