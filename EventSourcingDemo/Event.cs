namespace EventSourcingDemo;

public interface Event
{
    void Acept(EventVisitor visitor);
}


public interface EventVisitor
{
    void Visit(DrinksOrdered tableReserved);
    void Visit(TableReserved tableReserved);
}