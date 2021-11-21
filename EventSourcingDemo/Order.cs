namespace EventSourcingDemo
{
    public class Order
    {
        public string ProductName { get; init; }
        public int ProductId { get; init; }
        public int Quantity { get; init; }
        public int Price { get; init; }
        public string Comment { get; init; }
    }
}
