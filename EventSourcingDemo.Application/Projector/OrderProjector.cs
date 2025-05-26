using MediatR;

namespace EventSourcingDemo.Application.Projector;

public interface OrderCollection
{
    Task AddOrderAsync(PublicEvents.DrinksOrdered orderEvent);
    Task SetToServed(Guid orderId);
    Task<IEnumerable<Query.Order>> GetAsync();
}

public class OrderProjector(OrderCollection orderCollection) : INotificationHandler<PublicEvents.DrinksOrdered>, INotificationHandler<PublicEvents.DrinksServed>
{
    public async Task Handle(PublicEvents.DrinksOrdered notification, CancellationToken cancellationToken)
    {
        await orderCollection.AddOrderAsync(notification);
    }

    public async Task Handle(PublicEvents.DrinksServed notification, CancellationToken cancellationToken)
    {
        await orderCollection.SetToServed(notification.OrderId);
    }
}
