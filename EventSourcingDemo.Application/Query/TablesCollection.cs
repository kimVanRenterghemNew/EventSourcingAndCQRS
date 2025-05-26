namespace EventSourcingDemo.Application.Query;

public interface TablesCollection
{
    Task<IEnumerable<Table>> GetAsync(Between requestRange);
    Task<Table> GetAsync(int tableId);
    Task UpdateAsync(Table table);
}
