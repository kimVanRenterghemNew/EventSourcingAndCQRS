using EventSourcingDemo.Application.Projector;
using MediatR;

namespace EventSourcingDemo.Application.Query;
public record GetOrders() : IRequest<IEnumerable<Order>>;




public class GetOrdersHandler : IRequestHandler<GetOrders, IEnumerable<Order>>
{
    private readonly OrderCollection _orderCollection;
    public GetOrdersHandler(OrderCollection orderCollection)
    {
        _orderCollection = orderCollection;
    }

    public async Task<IEnumerable<Order>> Handle(GetOrders request, CancellationToken cancellationToken)
    {
        return await _orderCollection.GetAsync();
    }
}