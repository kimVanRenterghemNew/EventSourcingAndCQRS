using MediatR;
using MongoDB.Bson;

namespace EventSourcingDemo.MongoDb
{
    public class MongoEventVisitor(string reservationId, Table table) : EventVisitor
    {
        private BsonDocument? _doc;
        private INotification _event;

        public void Visit(DrinksOrdered drinksOrdered)
        {
            var orderDoc = new BsonDocument
            {
                    { "ProductName", drinksOrdered.Order.ProductName },
                    { "ProductId", drinksOrdered.Order.ProductId },
                    { "Quantity", drinksOrdered.Order.Quantity },
                    { "Price", drinksOrdered.Order.Price },
                    { "Comment", drinksOrdered.Order.Comment }
            };
            var eventDoc = new BsonDocument
            {
                    { "EventType", nameof(DrinksOrdered) },
                    { "Order", orderDoc }
            };
            _doc = new BsonDocument
            {
                    { "event", eventDoc },
                    { "metadata", new BsonDocument {
                            { "EventName", nameof(DrinksOrdered) },
                            { "CurrentDateTime", DateTime.UtcNow },
                            { "ReservationId", reservationId }
                    }}
            };
            _event = new PublicEvetns.DrinksOrdered(Guid.Parse(reservationId), drinksOrdered.Order, table.TableId, table.Name);
        }

        public void Visit(TableReserved tableReserved)
        {
            var eventDoc = new BsonDocument
            {
                    { "EventType", nameof(TableReserved) },
                    { "ReservationId", tableReserved.ReservationId.ToString() },
                    { "TableId", tableReserved.TableId },
                    { "Name", tableReserved.Name },
                    { "DateTime", tableReserved.DateTime },
                    { "NrOfGuests", tableReserved.NrOfGuests }
            };
            _doc = new BsonDocument
            {
                    { "event", eventDoc },
                    { "metadata", new BsonDocument {
                            { "EventName", nameof(TableReserved) },
                            { "CurrentDateTime", DateTime.UtcNow },
                            { "ReservationId", reservationId }
                    }}
            };
            _event = new PublicEvetns.TableReserved(Guid.Parse(reservationId), tableReserved.TableId,
                                                                      tableReserved.Name, tableReserved.DateTime,
                                                                      tableReserved.NrOfGuests);
        }

        public (BsonDocument, INotification) Transform(TableEvent e)
        {
            _doc = null;
            _event = null;
            e.Acept(this);
            if(_doc == null || _event == null)
                throw new InvalidOperationException($"Transform method did not create a valid event for {e.GetType().FullName}");

            return (_doc, _event);
        }
    }
}