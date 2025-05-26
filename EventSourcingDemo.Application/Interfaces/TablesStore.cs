namespace EventSourcingDemo.Application.Interfaces;

public interface TablesStore
{
    Task SaveAsync(Table table);
    Task<Table> Get(Guid requestReservationId);
}
