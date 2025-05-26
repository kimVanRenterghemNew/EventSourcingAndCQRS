using MediatR;

namespace EventSourcingDemo.Application.Query;
public record GetTables(Between Range) : IRequest<IEnumerable<Table>>;


public record Between(DateTime StartDate, DateTime EndDate);


public class GetTablesHandler : IRequestHandler<GetTables, IEnumerable<Table>>
{
    private readonly TablesCollection _tablesCollection;
    public GetTablesHandler(TablesCollection tablesCollection)
    {
        _tablesCollection = tablesCollection;
    }
    public async Task<IEnumerable<Table>> Handle(GetTables request, CancellationToken cancellationToken)
    {
        return await _tablesCollection.GetAsync(request.Range);
    }
}