using Xunit;
using EventSourcingDemo;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FluentAssertions;
using System;

namespace ventSourcingDemo.Test
{
    public class UnitTest1
    {
        [Fact]
        public async ValueTask Should_Emit_TableReservedEvent_On_Create()
        {
            var aggregate = new Table(6, new (2021, 12, 31), "kim vr", 2);
            var events = new List<TableEvents>();
            await aggregate.PlayAllEvents(async e =>
            {//because normally this is an async write to the db
                await Task.FromResult(0);
                events.Add(e);
            });

            events.Should().ContainSingle();

            events[0]
                .Should()
                .BeEquivalentTo(new TableReserved(
                    TableId: 6,
                    Name: "kim vr",
                    DateTime: new (2021, 12, 31),
                    NrOfGuests: 2));
        }

        [Fact]
        public async ValueTask Should_be_able_to_order_a_drink()
        {
            var events = new TableEvents[]
            {
                new TableReserved(
                    TableId: 6,
                    Name: "kim vr",
                    DateTime: new (2021, 12, 31),
                    NrOfGuests: 2
                )
            };

            var aggregate = new Table(events);
            aggregate.OrderDrinks(new Order(
                ProductName: "Martini",
                ProductId: 6,
                Quantity: 2,
                Price: 6,
                Comment: ""
                )
            );

            var newEvents = new List<TableEvents>();
            await aggregate.PlayAllEvents(async e =>
            {
                await Task.FromResult(0);
                newEvents.Add(e);
            });

            newEvents.Should().ContainSingle();
            newEvents[0]
                .Should()
                .BeEquivalentTo(
                    new DrinksOrdered(
                        new Order(
                            ProductName: "Martini",
                            ProductId: 6,
                            Quantity: 2,
                            Price: 6,
                            Comment: ""
                        )
                    )
                );
        }

        [Fact]
        public async ValueTask Should_only_allaw_2_times_a_drinks_order()
        {
            var events = new TableEvents[]
            {
                new TableReserved(
                    TableId: 6,
                    Name: "kim vr",
                    DateTime: new (2021, 12, 31),
                    NrOfGuests: 2
                ),
                new DrinksOrdered(
                    new Order(
                        ProductName: "Aperitif mison",
                        ProductId: 6,
                        Quantity: 2,
                        Price: 6,
                        Comment: ""
                    )
                ),
                new DrinksOrdered(
                    new Order(
                        ProductName: "Aperitif mison",
                        ProductId: 6,
                        Quantity: 2,
                        Price: 6,
                        Comment: ""
                    )
                ),
            };
            var newEvents = new List<TableEvents>();

            var aggregate = new Table(events);
            var act = () => aggregate.OrderDrinks(
                new Order(
                    ProductName: "martinie",
                    ProductId: 8,
                    Quantity: 2,
                    Price: 14,
                    Comment: ""
                ));

            act.Should()
                .Throw<Exception>()
                .WithMessage("Too many drinks :-)");

            await aggregate.PlayAllEvents(async e =>
            {
                await Task.FromResult(0);
                newEvents.Add(e);
            });

            newEvents.Any().Should().BeFalse();
        }
    }
}