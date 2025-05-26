using EventSourcingDemo.Application.Interfaces;
using EventSourcingDemo.Application.Query;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSourcingDemo.MongoDb;

public class MongoDbTablesRepository : TablesCollection
{
    private readonly IMongoCollection<BsonDocument> _collection;


    public async Task SeedAsync()
    {
        var tables = Enumerable.Range(1, 5)
                               .Select(i => new Application.Query.Table(i, []))
                               .Select(async t => SaveAsync(t))
                               .ToArray();

        await Task.WhenAll(tables);
    }


    public MongoDbTablesRepository()
    {
        var client = new MongoClient("mongodb://localhost:27018");
        var database = client.GetDatabase("EventSourcingDemo");
        _collection = database.GetCollection<BsonDocument>("teblesProjection");
    }

    public async Task SaveAsync(Application.Query.Table table)
    {
        var doc = new BsonDocument
        {
            { "tableId", table.Id },
            { "reservations", new BsonArray(table.Reservations.Select(r => new BsonDocument {
                { "reservationId", r.ReservationId.ToString() },
                { "name", r.Name },
                { "dateTime", r.DateTime },
                { "nrOfGuests", r.NrOfGuests },
                { "cost", r.TotalCost }
            }))}
        };
        await _collection.InsertOneAsync(doc);
    }

    public async Task<IEnumerable<Application.Query.Table>> GetAsync(Between requestRange)
    {
        var docs = await _collection.Find(Builders<BsonDocument>.Filter.Empty).ToListAsync();

        var tables = docs.Select(doc =>
        {
            var tableId = doc.GetValue("tableId", 0).AsInt32;
            var reservations = new List<Reservation>();
            if (doc.Contains("reservations"))
            {
                foreach (var resDoc in doc["reservations"].AsBsonArray)
                {
                    var r = resDoc.AsBsonDocument;
                    reservations.Add(new Reservation(
                        Guid.Parse(r["reservationId"].AsString),
                        r["name"].AsString,
                        r["dateTime"].ToUniversalTime(),
                        r["nrOfGuests"].AsInt32,
                        r.GetValue("cost", 0).AsDouble
                    ));
                }
            }
            return new Application.Query.Table(tableId, reservations);
        });
        return tables;
    }

    public async Task<Application.Query.Table> GetAsync(int tableId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("tableId", tableId);
        var doc = await _collection.Find(filter).FirstOrDefaultAsync();
        if (doc == null)
            throw new KeyNotFoundException($"Table with id {tableId} not found");

        var reservations = new List<Reservation>();
        if (doc.Contains("reservations"))
        {
            foreach (var resDoc in doc["reservations"].AsBsonArray)
            {
                var r = resDoc.AsBsonDocument;
                reservations.Add(new Reservation(
                    Guid.Parse(r["reservationId"].AsString),
                    r["name"].AsString,
                    r["dateTime"].ToUniversalTime(),
                    r["nrOfGuests"].AsInt32,
                    r.GetValue("cost", 0).AsDouble
                ));
            }
        }
        return new (tableId, reservations);
    }

    public async Task UpdateAsync(Application.Query.Table table)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("tableId", table.Id);
        var doc = new BsonDocument
        {
            { "tableId", table.Id },
            { "reservations", new BsonArray(table.Reservations.Select(r => new BsonDocument {
                { "reservationId", r.ReservationId.ToString() },
                { "name", r.Name },
                { "dateTime", r.DateTime },
                { "nrOfGuests", r.NrOfGuests },
                { "cost", r.TotalCost }
            }))}
        };
        await _collection.ReplaceOneAsync(filter, doc, new ReplaceOptions { IsUpsert = true });
    }
}