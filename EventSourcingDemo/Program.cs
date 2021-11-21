// See https://aka.ms/new-console-template for more information
using EventSourcingDemo;

Console.WriteLine("Hello, World!");

var events = new ITableEvents[]
           {
                new TableReserved
                {
                    TableId = 6,
                    DateTime = new DateTime(2021, 12, 31),
                    Name = "kim vr",
                    NrOfGuests = 2
                },
                new DrinksOrdered {
                    Order = new Order
                        {
                            ProductId = 6,
                            Price = 6,
                            ProductName = "Aperitif mison",
                            Quantity = 2
                        }
                },
                new DrinksOrdered {
                    Order = new Order
                        {
                            ProductId = 6,
                            Price = 6,
                            ProductName = "Aperitif mison",
                            Quantity = 2
                        }
                }
           };
var newEvents = new List<ITableEvents>();

//get aggregate in repository
var aggregate = new Table(events);

//execute command
aggregate.OrderDrinks(new Order
{
    ProductId = 6,
    Price = 6,
    ProductName = "martinie",
    Quantity = 2
});

//commit events in repository
await aggregate.PlayAllEvents(async e =>
{
    await Task.FromResult(0);
    newEvents.Add(e);
});