using EventSourcingDemo.Application.Commands;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSourcingDemo.MongoDb;

public class MongoDbTablesStore : TablesStore
{
    private readonly IMediator _mediator;
    private readonly IMongoCollection<BsonDocument> _collection;

    public MongoDbTablesStore(IMediator mediator)
    {
        _mediator = mediator;
        var client = new MongoClient("mongodb://localhost:27018");
        var database = client.GetDatabase("EventSourcingDemo");
        _collection = database.GetCollection<BsonDocument>("tebleReservationStream");
    }

    public async Task SaveAsync(Table table)
    {
        var visitor = new MongoEventVisitor(table.ReservationId.ToString(), table);

        await table.PlayAllEvents(async e =>
        {
            var (doc, @event) = visitor.Transform(e);
            await _collection.InsertOneAsync(doc);

            await _mediator.Publish(@event);
        });
    }

    public async Task<Table> Get(Guid requestReservationId)
    {
        var docs = await GetAlleventsForReservation(requestReservationId);

        var events = docs.Select(ParseTableEvent).ToList();

        return new Table(events);
    }

    private async Task<List<BsonDocument>> GetAlleventsForReservation(Guid requestReservationId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("metadata.ReservationId", requestReservationId.ToString());
        var sort = Builders<BsonDocument>.Sort.Ascending("metadata.CurrentDateTime");
        var docs = await _collection.Find(filter).Sort(sort).ToListAsync();
        return docs;
    }
    private TableEvent ParseTableEvent(BsonDocument doc)
    {
        var metadata = doc["metadata"].AsBsonDocument;
        var eventName = metadata["EventName"].AsString;
        var eventDoc = doc["event"].AsBsonDocument;
        return eventName switch
        {
            nameof(TableReserved) => new TableReserved(
                Guid.Parse(eventDoc["ReservationId"].AsString),
                eventDoc["TableId"].AsInt32,
                eventDoc["Name"].AsString,
                eventDoc["DateTime"].ToUniversalTime(),
                eventDoc["NrOfGuests"].AsInt32
            ),
            nameof(DrinksOrdered) => new DrinksOrdered(
                new Order(
                    eventDoc["Order"]["ProductName"].AsString,
                    eventDoc["Order"]["ProductId"].AsInt32,
                    eventDoc["Order"]["Quantity"].AsInt32,
                    eventDoc["Order"]["Price"].AsDouble,
                    eventDoc["Order"].AsBsonDocument.Contains("Comment") ? eventDoc["Order"]["Comment"].AsString : string.Empty
                )
            ),
            _ => throw new NotSupportedException($"Unknown event type: {eventName}")
        };
    }
}