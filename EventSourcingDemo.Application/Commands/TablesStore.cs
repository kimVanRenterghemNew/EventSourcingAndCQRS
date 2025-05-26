namespace EventSourcingDemo.Application.Commands;

public interface TablesStore
{
    Task SaveAsync(Table table);
    Task<Table> Get(Guid requestReservationId);
}
