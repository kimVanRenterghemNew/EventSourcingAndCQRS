namespace EventSourcingDemo;
public class Table : BaseAggregate<TableEvents>
{
    public int TableId { get; private set; }
    public string Name { get; private set; }
    public DateTime DateTime { get; private set; }
    public int NrOfGuests { get; private set; }
    public int TotallBill { get; private set; }

    private readonly List<Order> _orders = new();
    private int _nrOfDrinksOrdered = 0;

    public Table(int tableId, DateTime dateTime, string name, int nrOfGuests)
    {
        RegisterHandlers();
        PublishNewEvent(new TableReserved(tableId, name, dateTime, nrOfGuests));
    }

    public Table(IEnumerable<TableEvents> events)
    {
        RegisterHandlers();

        //play all the evetns
        events
            .ToList()
            .ForEach(PlayEvent);
    }

    //command
    public void OrderDrinks(Order order)
    {
        if (_nrOfDrinksOrdered >= 2 * NrOfGuests)
            throw new Exception("Too many drinks :-)");

        PublishNewEvent(new DrinksOrdered(order));
    }

    // register all events handlers
    private void RegisterHandlers()
    {
        RegisterHandler<TableReserved>(TableReservedHandler);
        RegisterHandler<DrinksOrdered>(DrinksOrderedHandler);
    }

    //play the event
    private void DrinksOrderedHandler(DrinksOrdered @event)
    {
        _orders.Add(@event.Order);
        _nrOfDrinksOrdered += @event.Order.Quantity;
        TotallBill += @event.Order.Price;
    }

    private void TableReservedHandler(TableReserved @event)
    {
        TableId = @event.TableId;
        Name = @event.Name;
        DateTime = @event.DateTime;
        NrOfGuests = @event.NrOfGuests;
    }
}
