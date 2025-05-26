namespace EventSourcingDemo;

public interface Event
{
    void Accept(EventVisitor visitor);
}


public interface EventVisitor
{
    void Visit(DrinksOrdered ordered);
    void Visit(TableReserved tableReserved);
    void Visit(DrinksServed served);
}