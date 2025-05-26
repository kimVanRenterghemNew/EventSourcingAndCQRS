namespace EventSourcingDemo.Application.Interfaces;

public interface OrderCollection
{
    Task AddOrderAsync(PublicEvents.DrinksOrdered orderEvent);
    Task SetToServed(Guid orderId);
    Task<IEnumerable<Query.Order>> GetAsync();
}