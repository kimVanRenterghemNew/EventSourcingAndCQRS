using EventSourcingDemo.Application.Interfaces;
using MediatR;

namespace EventSourcingDemo.Application.Query;
public record GetTables(Between Range) : IRequest<IEnumerable<Table>>;


public record Between(DateTime StartDate, DateTime EndDate);


public class GetTablesHandler(TablesCollection tablesCollection) : IRequestHandler<GetTables, IEnumerable<Table>>
{
    public async Task<IEnumerable<Table>> Handle(GetTables request, CancellationToken cancellationToken)
    {
        return await tablesCollection.GetAsync(request.Range);
    }
}