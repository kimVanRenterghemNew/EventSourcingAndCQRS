namespace EventSourcingDemo
{
    public class TableReserved : ITableEvents
    {
        public int TableId { get; init; }
        public DateTime DateTime { get; init; }
        public string Name { get; init; }
        public int NrOfGuests { get; init; }
    }
}
