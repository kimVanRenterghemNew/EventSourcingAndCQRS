using EventSourcingDemo.Application.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSourcingDemo.MongoDb;

public class MongoDbOrderRepository : OrderCollection
{
    private readonly IMongoCollection<BsonDocument> _collection;

    public MongoDbOrderRepository()
    {
        var client = new MongoClient("mongodb://localhost:27018");
        var database = client.GetDatabase("EventSourcingDemo");
        _collection = database.GetCollection<BsonDocument>("orders");
    }

    public async Task AddOrderAsync(PublicEvents.DrinksOrdered orderEvent)
    {
        var order = orderEvent.Order;
        var doc = new BsonDocument
        {
            { "orderId", orderEvent.OrderId.ToString() },
            { "reservationId", orderEvent.ReservationId.ToString() },
            { "tableId", orderEvent.TableId },
            { "productName", order.ProductName },
            { "productId", order.ProductId },
            { "quantity", order.Quantity },
            { "comment", order.Comment },
            { "status", "inque" }
        };
        await _collection.InsertOneAsync(doc);
    }

    public async Task SetToServed(Guid orderId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("orderId", orderId.ToString());
        var update = Builders<BsonDocument>.Update.Set("status", "served");
        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task<IEnumerable<Application.Query.Order>> GetAsync()
    {
        var docs = await _collection.Find(Builders<BsonDocument>.Filter.Empty).ToListAsync();
        var orders = docs
            .Where(doc => doc.Contains("orderId") && Guid.TryParse(doc["orderId"].AsString, out _))
            .Select(doc => new Application.Query.Order(
                Guid.Parse(doc["orderId"].AsString),
                Guid.Parse(doc.GetValue("reservationId", "00000000-0000-0000-0000-000000000000").AsString),
                doc.GetValue("tableId", 0).AsInt32,
                doc.GetValue("productName", "").AsString,
                doc.GetValue("productId", 0).AsInt32,
                doc.GetValue("quantity", 0).AsInt32,
                doc.GetValue("comment", "").AsString,
                doc.GetValue("status", "InQueue").AsString 
            ));
        return orders;
    }
}
