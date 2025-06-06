﻿namespace EventSourcingDemo;
public class Table : BaseAggregate<TableEvent>
{
    public Guid ReservationId { get; private set; }
    public int TableId { get; private set; }
    public string Name { get; private set; }
    public DateTime DateTime { get; private set; }
    public int NrOfGuests { get; private set; }
    public double TotalBill { get; private set; } = 0.0;

    private readonly List<Order> _orders = [];

    private int _nrOfDrinksOrdered = 0;

    public Table(int tableId, DateTime dateTime, string name, int nrOfGuests)
    {
        RegisterHandlers();
        PublishNewEvent(new TableReserved(Guid.NewGuid() , tableId, name, dateTime, nrOfGuests));
    }

    public Table(IEnumerable<TableEvent> events)
    {
        RegisterHandlers();

        //play all the evens
        events
            .ToList()
            .ForEach(PlayEvent);
    }

    //command
    public void OrderDrinks(Order order)
    {
        if (_nrOfDrinksOrdered >= 2 * NrOfGuests)
            throw new ArgumentException("Too many drinks :-)");

        PublishNewEvent(new DrinksOrdered(order));
    }
    public void ServeDrinks(Guid order)
    {
        var existingOrder = _orders.FirstOrDefault(o => o.OrderId == order);
        
        if (existingOrder == null)
        {
            throw new ArgumentException("This order dos not exists.");
        }
        PublishNewEvent(new DrinksServed(order));
    }
    // register all events handlers
    private void RegisterHandlers()
    {
        RegisterHandler<TableReserved>(TableReservedHandler);
        RegisterHandler<DrinksOrdered>(DrinksOrderedHandler);
        RegisterHandler<DrinksServed>(DrinksServedHandler);
    }

    //play the event
    private void DrinksOrderedHandler(DrinksOrdered @event)
    {
        _orders.Add(@event.Order);
        _nrOfDrinksOrdered += @event.Order.Quantity;
        TotalBill += @event.Order.Price;
    }

    private void TableReservedHandler(TableReserved @event)
    {
        ReservationId = @event.ReservationId;
        TableId = @event.TableId;
        Name = @event.Name;
        DateTime = @event.DateTime;
        NrOfGuests = @event.NrOfGuests;
    }

    private void DrinksServedHandler(DrinksServed served)
    {
        var existingOrder = _orders.FirstOrDefault(o => o.OrderId == served.Order);
        _orders.Remove(existingOrder);
    }
}
