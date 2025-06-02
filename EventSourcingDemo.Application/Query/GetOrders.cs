using EventSourcingDemo.Application.Interfaces;
using MediatR;

namespace EventSourcingDemo.Application.Query;

public record GetOrders() : IRequest<IEnumerable<Order>>;

public class GetOrdersHandler(OrderCollection orderCollection) : IRequestHandler<GetOrders, IEnumerable<Order>>
{
    public async Task<IEnumerable<Order>> Handle(GetOrders request, CancellationToken cancellationToken)
    {
        return await orderCollection.GetAsync();
    }
}