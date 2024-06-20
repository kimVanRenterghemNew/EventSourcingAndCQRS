// See https://aka.ms/new-console-template for more information

using EventSourcingDemo;

Console.WriteLine("Hello, World!");
var id = "225635";

var newEvents = new List<TableEvents>();

//get aggregate in repository
var events = GetTable(id);
var table = new Table(events);

//execute command
table.OrderDrinks(new Order(
    ProductName: "martinie",
    ProductId: 6,
    Price: 6,
    Quantity: 2,
    Comment: ""
    ));

//commit events in repository
await table.PlayAllEvents(AddEvent);




async Task AddEvent(TableEvents e)
{
    await Task.FromResult(0);
    newEvents.Add(e);
}

TableEvents[] GetTable(string id)
{
    var tableEventsArray = new TableEvents[]
    {
        new TableReserved(
            TableId: 6,
            Name: "kim vr",
            DateTime: new(2021, 12, 31),
            NrOfGuests: 2
        ),
        new DrinksOrdered (
            new Order(
                ProductName: "Aperitif mison",
                ProductId: 6,
                Price: 6,
                Quantity: 2,
                Comment:""
            )
        ),
        new DrinksOrdered(
            new Order(
                ProductId: 6,
                Price: 6,
                ProductName: "Aperitif mison",
                Quantity: 2,
                Comment:""
            )
        )
    };
    return tableEventsArray;
}