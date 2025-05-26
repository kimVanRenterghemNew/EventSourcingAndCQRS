using EventSourcingDemo.Application.Query;

namespace EventSourcingDemo.Application.Interfaces;

public interface TablesCollection
{
    Task<IEnumerable<Query.Table>> GetAsync(Between requestRange);
    Task<Query.Table> GetAsync(int tableId);
    Task UpdateAsync(Query.Table table);
}
