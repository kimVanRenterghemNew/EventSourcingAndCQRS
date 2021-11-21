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
        public async void Should_Emit_TableReservedEvent_On_Create()
        {
            var aggregate = new Table(6, new DateTime(2021, 12, 31), "kim vr", 2);
            var events = new List<ITableEvents>();
            await aggregate.PlayAllEvents(async e =>
            {//because normally this is an async write to the db
                await Task.FromResult(0);
                events.Add(e);
            });

            events.Should().ContainSingle();

            events.First()
                .Should().Equals(new TableReserved
                {
                    TableId = 6,
                    DateTime = new System.DateTime(2021, 12, 31),
                    Name = "kim vr",
                    NrOfGuests = 2
                });
        }

        [Fact]
        public async void Should_be_able_to_order_a_drink()
        {
            var events = new ITableEvents[]
            {
                new TableReserved
                {
                    TableId = 6,
                    DateTime = new DateTime(2021, 12, 31),
                    Name = "kim vr",
                    NrOfGuests = 2
                }
            };

            var aggregate = new Table(events);
            aggregate.OrderDrinks(new Order
            {
                ProductId = 6,
                Price = 6,
                ProductName = "Martini",
                Quantity = 2
            });

            var newEvents = new List<ITableEvents>();
            await aggregate.PlayAllEvents(async e =>
            {
                await Task.FromResult(0);
                newEvents.Add(e);
            });

            newEvents.Should().ContainSingle();
            newEvents.First().Should().Equals(new DrinksOrdered
            {
                Order = new Order
                {
                    ProductId = 6,
                    Price = 6,
                    ProductName = "Martini",
                    Quantity = 2
                }
            });
        }

        [Fact]
        public async void Should_only_allaw_2_times_a_drinks_order()
        {
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

            var aggregate = new Table(events);
            Action act = () => aggregate.OrderDrinks(new Order
            {
                ProductId = 6,
                Price = 6,
                ProductName = "martinie",
                Quantity = 2
            });
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